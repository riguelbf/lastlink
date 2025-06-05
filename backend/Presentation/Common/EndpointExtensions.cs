using Microsoft.AspNetCore.Routing;
using Presentation.HealthChecks;

namespace Presentation.Common;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Map health check endpoint
        endpoints.MapHealthCheckEndpoint();
        
        // Map other API endpoints here
        // Example: endpoints.MapAdvanceRequestEndpoints();
        
        return endpoints;
    }
    
    public static WebApplication UseApiEndpoints(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}
