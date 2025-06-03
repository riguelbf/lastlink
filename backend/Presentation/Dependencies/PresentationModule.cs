namespace Presentation.Dependencies;

public static class PresentationModule
{
    public static IServiceCollection AddPresentationModule(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddProblemDetails()
            .AddValidators();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services;
    }
}