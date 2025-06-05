using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation.HealthChecks;

public static class HealthCheckEndpoint
{
    public static void MapHealthCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/health", async context =>
        {
            // Add any additional health checks here
            await context.Response.WriteAsync("Healthy");
        });
    }
}
