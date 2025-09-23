using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace ModularMonolithTemplate.Infrastructure.Http.Endpoints;

/// <summary>
/// Contract implemented by modules to register their minimal API endpoints.
/// </summary>
public interface IEndpointRegistrar
{
    void MapEndpoints(IEndpointRouteBuilder app);
}

public static class EndpointDiscoveryExtensions
{
    /// <summary>
    /// Scans loaded assemblies for implementations of <see cref="IEndpointRegistrar"/> and invokes them.
    /// </summary>
    public static void MapDiscoveredEndpoints(this IEndpointRouteBuilder app)
    {
        var registrarType = typeof(IEndpointRegistrar);

        var registrars = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(SafeGetTypes)
            .Where(t => registrarType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IEndpointRegistrar)
            .Where(r => r is not null)!
            .ToList();

        foreach (var registrar in registrars)
        {
            registrar.MapEndpoints(app);
        }
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try { return assembly.GetTypes(); }
        catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t is not null)!; }
    }
}
