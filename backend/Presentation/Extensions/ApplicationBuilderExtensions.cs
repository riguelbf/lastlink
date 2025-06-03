namespace Presentation.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Adds Swagger support to the application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>The application.</returns>
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}