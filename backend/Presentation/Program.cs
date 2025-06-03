using System.Reflection;
using Application.Dependencies;
using Asp.Versioning;
using DotNetEnv;
using HealthChecks.UI.Client;
using Infrastructure.Dependencies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation.Dependencies;
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

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.WebHost.UseUrls("http://0.0.0.0:5053");

builder.Host.UseSerilog();

builder.Services.AddSwaggerGen();

builder.Services
    .AddApplicationModule()
    .AddPresentationModule()
    .AddInfrastructureModule(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        
        policy
            .WithOrigins(frontendUrl ?? string.Empty)
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseGlobalExceptionHandling();

await app.RunAsync();

namespace Presentation
{
    public partial class Program;
}