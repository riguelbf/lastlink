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
        services
            .AddQueryHandlers()
            .AddCommandHandlers();
        
        return services;
    }

    /// <summary>
    /// Scans the assembly for all classes implementing IQueryHandler and registers them as scoped services.
    /// </summary>
    /// <param name="services">The IServiceCollection to add query handlers to.</param>
    /// <returns>The updated IServiceCollection.</returns>
    private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationModule))
            .AddClasses(classes => classes.AssignableTo(typeof(Products.Queries.IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
    
    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationModule))
            .AddClasses(classes => classes.AssignableTo(typeof(Products.Commands.ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}