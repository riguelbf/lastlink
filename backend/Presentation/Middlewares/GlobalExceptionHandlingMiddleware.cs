using Serilog;
using System.Net;

namespace Presentation.Middlewares
{
    public class GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IDiagnosticContext diagnosticContext)
    {
        private readonly IDiagnosticContext _diagnosticContext = diagnosticContext;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred while processing request");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{\"error\":\"{WebUtility.HtmlEncode(ex.Message)}\"}}");
            }
        }
    }
}
