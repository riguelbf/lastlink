using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Infrastructure.Config
{
    // Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
    // This project should be referenced by each service project in your solution.
    // To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
    public static class Extensions
    {
        private const string HealthEndpointPath = "/health";
        private const string AlivenessEndpointPath = "/alive";

        public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.ConfigureOpenTelemetry();

            builder.AddDefaultHealthChecks();

            builder.Services.AddServiceDiscovery();

            builder.Services.ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });

            return builder;
        }

        public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            // Logging pipeline defaults
            builder.Logging.AddDefaultOpenTelemetry();

            // Metrics and tracing defaults
            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddDefaultAspNetMetrics();
                })
                .WithTracing(tracing =>
                {
                    tracing.AddDefaultAspNetTracing(
                        builder.Environment.ApplicationName,
                        HealthEndpointPath,
                        AlivenessEndpointPath);
                });

            builder.AddOpenTelemetryExporters();

            return builder;
        }

        private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            if (useOtlpExporter)
            {
                builder.Services.AddOpenTelemetry().UseOtlpExporter();
            }

            // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
            //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
            //{
            //    builder.Services.AddOpenTelemetry()
            //       .UseAzureMonitor();
            //}

            return builder;
        }

        public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddHealthChecks()
                // Add a default liveness check to ensure app is responsive
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return builder;
        }

        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            // Adding health checks endpoints to applications in non-development environments has security implications.
            // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
            if (app.Environment.IsDevelopment())
            {
                // All health checks must pass for app to be considered ready to accept traffic after starting
                app.MapHealthChecks(HealthEndpointPath);

                // Only health checks tagged with the "live" tag must pass for app to be considered alive
                app.MapHealthChecks(AlivenessEndpointPath, new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("live")
                });
            }

            return app;
        }
    }
}

// Expose logging defaults as extension methods on ILoggingBuilder
namespace Microsoft.Extensions.Logging
{
    public static class LoggingExtensions
    {
        public static ILoggingBuilder AddDefaultOpenTelemetry(this ILoggingBuilder logging)
        {
            logging.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
            });
            return logging;
        }
    }
}

// Expose metrics defaults as extension methods on MeterProviderBuilder
namespace OpenTelemetry.Metrics
{
    public static class MetricsExtensions
    {
        public static MeterProviderBuilder AddDefaultAspNetMetrics(this MeterProviderBuilder metrics)
        {
            metrics.AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
                   .AddRuntimeInstrumentation();
            return metrics;
        }
    }
}

// Expose tracing defaults as extension methods on TracerProviderBuilder
namespace OpenTelemetry.Trace
{
    public static class TracingExtensions
    {
        public static TracerProviderBuilder AddDefaultAspNetTracing(
            this TracerProviderBuilder tracing,
            string applicationName,
            string healthEndpointPath = "/health",
            string alivenessEndpointPath = "/alive")
        {
            tracing.AddSource(applicationName)
                   .AddAspNetCoreInstrumentation(options =>
                   {
                       // Exclude health check requests from tracing
                       options.Filter = context =>
                           !context.Request.Path.StartsWithSegments(healthEndpointPath)
                           && !context.Request.Path.StartsWithSegments(alivenessEndpointPath);
                   })
                   // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                   //.AddGrpcClientInstrumentation()
                   .AddHttpClientInstrumentation();
            return tracing;
        }
    }
}
