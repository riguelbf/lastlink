namespace Presentation.Endpoints;

public static class HealthCheckEndpoint
{
    public static void MapHealthCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/health", async context =>
        {
            await context.Response.WriteAsync("Healthy");
        });
    }
}
