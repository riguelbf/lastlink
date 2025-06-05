using Application.AdvanceRequests.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Dependencies;

/// <summary>
/// Provides extension methods to register application-level dependencies, including
/// automatic discovery and registration of query handler services using Scrutor.
/// </summary>
public static class ApplicationModule
{
    /// <summary>
    /// Registers all application services, including query handlers.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddAdvanceRequestsModule();
        
        return services;
    }
}