using System.Reflection;
using Application.Dependencies;
using Asp.Versioning;
using DotNetEnv;
using HealthChecks.UI.Client;
using Infrastructure.Dependencies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation.Dependencies;
using Presentation.Endpoints;
using Presentation.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Env.Load();


var builder = WebApplication.CreateBuilder(args);

///////////////////////////////////////////////////////////////////////////////////
// Builder configuration //////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////

builder.WebHost.UseUrls("http://0.0.0.0:5053");

builder.Host.UseSerilog();

builder.Services.AddSwaggerGen();

// Configure services
builder.Services
    .AddInfrastructureModule()
    .AddApplicationModule()
    .AddPresentationModule();
    
// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        if (!string.IsNullOrEmpty(frontendUrl))
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "LastLink API", Version = "v1" });
    
    // Include XML comments for API documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure health checks
builder.Services.AddHealthChecks();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

///////////////////////////////////////////////////////////////////////////////////
// App configuration /////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LastLink API V1");
    });
}

app.MapEndpoints();
app.UseHttpsRedirection();
app.UseCors();

// Map API endpoints
var api = app.NewVersionedApi();

// Configure health checks
app.MapHealthCheckEndpoint();

app.UseSerilogRequestLogging();
app.UseRequestContextLogging();
app.UseGlobalExceptionHandling();

await app.RunAsync();

namespace Presentation
{
    public partial class Program;
}