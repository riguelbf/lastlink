using Microsoft.Extensions.Hosting;
using Infrastructure.Config;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Background;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.OpenTelemetry;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Elasticsearch;
using Scalar.AspNetCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Diagnostics;
using Api.Middleware;
using ModularMonolith.Platform.SharedKernel.Messaging;
using OpenTelemetry.Logs;
using WebApp.Infrastructure.Http;
using Modules.Billing;
using WebApp.Modules.Catalog;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env (if present)
EnvVars.Load(builder.Environment.ContentRootPath);

// Ensure W3C trace id format for consistent linking across OTel/Serilog/Aspire
Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Activity.ForceDefaultIdFormat = true;

// Serilog structured logging (JSON console for ELK/Grafana)
builder.Host.UseSerilog((ctx, services, cfg) =>
{
    cfg.MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("app", "webapp")
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithSpan()
        .WriteTo.Console(new CompactJsonFormatter());

    // Optional: Elasticsearch sink via env
    // LOG_ELASTICSEARCH_ENABLED=true
    // LOG_ELASTICSEARCH_NODE_URIS=http://localhost:9200
    // LOG_ELASTICSEARCH_INDEX_FORMAT=webapp-logs-{0:yyyy.MM}
    if (EnvVars.GetBool("LOG_ELASTICSEARCH_ENABLED"))
    {
        var nodeUris = EnvVars.Get("LOG_ELASTICSEARCH_NODE_URIS");
        var indexFormat = EnvVars.Get("LOG_ELASTICSEARCH_INDEX_FORMAT", "webapp-logs-{0:yyyy.MM}");
        if (!string.IsNullOrWhiteSpace(nodeUris))
        {
            var options = new ElasticsearchSinkOptions(new Uri(nodeUris))
            {
                AutoRegisterTemplate = true,
                IndexFormat = indexFormat!
            };
            cfg.WriteTo.Elasticsearch(options);
        }
    }

    // Optional: Grafana Loki sink via env
    // LOG_LOKI_ENABLED=true
    // LOG_LOKI_URI=http://localhost:3100
    if (EnvVars.GetBool("LOG_LOKI_ENABLED"))
    {
        var uri = EnvVars.Get("LOG_LOKI_URI");
        if (!string.IsNullOrWhiteSpace(uri))
        {
            var labels = new[]
            {
                new LokiLabel()
                {
                    Key = "app",
                    Value = "webapp"
                } ,
                new LokiLabel()
                {
                    Key = "env",
                    Value = ctx.HostingEnvironment.EnvironmentName
                }
            };
            cfg.WriteTo.GrafanaLoki(uri!, labels: labels, textFormatter: new CompactJsonFormatter());
        }
    }

    // Optional: OTLP logs export via Serilog sink (when Serilog replaces ILogger providers)
    // Honors OTEL_EXPORTER_OTLP_ENDPOINT and OTEL_EXPORTER_OTLP_PROTOCOL
    var otlpEndpoint = EnvVars.Get("OTEL_EXPORTER_OTLP_ENDPOINT");
    var otlpProtocol = (EnvVars.Get("OTEL_EXPORTER_OTLP_PROTOCOL", "grpc") ?? "grpc").ToLowerInvariant();
    if (!string.IsNullOrWhiteSpace(otlpEndpoint))
    {
        var protocol = otlpProtocol == "http/protobuf" || otlpProtocol == "http" ? OtlpProtocol.HttpProtobuf : OtlpProtocol.Grpc;
        cfg.WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = otlpEndpoint!;
            options.Protocol = protocol;
            options.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = builder.Environment.ApplicationName,
                ["service.instance.id"] = Environment.MachineName,
                ["service.version"] = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0"
            };
        });
    }
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

// Propagate correlation across outbound HTTP calls
builder.Services.AddHeaderPropagation(o =>
{
    o.Headers.Add("x-correlation-id");
});

// Typed HttpClient dynamic API with resilience and header propagation
builder.Services
    .AddHttpClient<IDynamicApi, DynamicApi>((sp, c) =>
    {
        var baseUrl = EnvVars.Get("DYNAMIC_API_BASE_URL");
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            c.BaseAddress = new Uri(baseUrl);
        }
        c.Timeout = TimeSpan.FromSeconds(30);
        c.DefaultRequestHeaders.Add("User-Agent", "lastlink-webapp/1.0");
    })
    .AddHeaderPropagation()
    .AddStandardResilienceHandler(options =>
    {
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(10);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
    });

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Aspire ServiceDefaults (health, logs, basic OTel exporters). Keep custom sources below.
builder.AddServiceDefaults();

// Event bus for in-process domain notifications
builder.Services.AddSingleton<IEventBus, InProcessEventBus>();

// OpenTelemetry tracing with module ActivitySources (ensure Aspire sees all spans)
builder.Services.AddOpenTelemetry()
    .WithTracing(b => b
        .ConfigureResource(rb => rb
            .AddService(
                serviceName: builder.Environment.ApplicationName,
                serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
                serviceInstanceId: Environment.MachineName))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddSource("Billing")
        .AddSource("Catalog")
        .AddSource("App.MediatR")
    );

// Note: OpenTelemetry logging (OTLP) is configured via ServiceDefaults (cross-cutting UseOtlpExporter).
// Avoid adding signal-specific AddOtlpExporter here to prevent conflicts.

// Background publisher for persisted domain notifications (outbox-like)
builder.Services.AddHostedService<DomainNotificationPublisher>();
builder.Services.AddOptions<DomainNotificationPublisherOptions>();

// Module registries
BillingModule.AddBilling(builder.Services, builder.Configuration);
CatalogModule.AddCatalog(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

// Development-only docs mapping and Scalar UI
if (app.Environment.IsDevelopment())
{
    // Single mapping that serves any {documentName} (e.g., v1) at /openapi/{documentName}.json
    app.MapOpenApi("/openapi/{documentName}.json");

    // Stable alias to v1 document
    app.MapGet("/openapi.json", () => Results.Redirect("/openapi/v1.json"));

    // Scalar pinned to v1 with custom options
    if (EnvVars.GetBool("SCALAR_ENABLED", true))
    {
        app.MapScalarApiReference("/api-docs/v1", opt =>
        {
            opt.Theme = ScalarTheme.Solarized;
            opt.WithTitle("Lastlink Payments API");
            opt.WithDefaultOpenAllTags(true);
            opt.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            opt.DotNetFlag = true;
        });
    }
}

app.UseHttpsRedirection();

// Structured request logging
app.UseSerilogRequestLogging();

// Inbound header propagation middleware
app.UseHeaderPropagation();

// Global exception handling
app.UseMiddleware<GlobalExceptionMiddleware>();

// Attribute-routed controllers
app.MapControllers();

// Map module endpoints (if using minimal APIs inside modules)
BillingModule.MapBilling(app);
CatalogModule.MapCatalog(app);

// Health checks
app.MapHealthChecks("/health");

app.Run();
