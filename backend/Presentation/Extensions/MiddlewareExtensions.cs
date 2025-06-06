using Presentation.Middlewares;

namespace Presentation.Extensions;

public static class MiddlewareExtensions
{
    public static void UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
    }

    public static void UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}