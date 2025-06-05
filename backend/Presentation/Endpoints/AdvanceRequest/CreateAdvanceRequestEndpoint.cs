using Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints.AdvanceRequest;

public class CreateAdvanceRequestEndpoint : EndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        var advanceRequests = app.MapGroup("/api/v1/advancerequests")
            .WithTags("Advance Requests")
            .WithOpenApi();

        advanceRequests.MapPost("/", HandleAsync)
            .WithName("CreateAdvanceRequest")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateAdvanceRequestCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        
        return result.Match(
            onSuccess: id => Results.CreatedAtRoute(
                routeName: "GetAdvanceRequestById",
                routeValues: new { id },
                value: new { id }),
            onFailure: error => error.Code switch
            {
                "Validation" => Results.BadRequest(new { error.Code, error.Message }),
                "Conflict" => Results.Conflict(new { error.Code, error.Message }),
                _ => Results.Problem(
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError)
            });
    }
}