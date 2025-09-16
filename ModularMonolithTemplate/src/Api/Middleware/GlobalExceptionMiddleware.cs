using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InvalidOperationException vex)
        {
            // Treat invalid operations (often mapping/validation) as 400
            logger.LogWarning(vex, "Invalid operation: {Message}", vex.Message);
            var problem = CreateProblem(context, (int)HttpStatusCode.BadRequest, "Invalid request", vex.Message);
            await WriteProblemAsync(context, problem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled server error");
            var problem = CreateProblem(context, (int)HttpStatusCode.InternalServerError, "Server error", "An unexpected error occurred.");
            await WriteProblemAsync(context, problem);
        }
    }

    private static ProblemDetails CreateProblem(HttpContext ctx, int statusCode, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = ctx.Request.Path,
            Extensions =
            {
                ["traceId"] = ctx.TraceIdentifier
            }
        };
    }

    private static async Task WriteProblemAsync(HttpContext context, ProblemDetails problem)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(problem);
    }
}
