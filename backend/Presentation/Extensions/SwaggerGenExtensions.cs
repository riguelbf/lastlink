namespace Presentation.Extensions;

internal static class SwaggerGenExtensions
{
    internal static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
        });

        return services;
    }
}