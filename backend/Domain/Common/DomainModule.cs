using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common;

/// <summary>
/// Registers domain services with the dependency injection container.
/// </summary>
public static class DomainModule
{
    /// <summary>
    /// Adds domain services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // Register domain services here
        
        return services;
    }
}
