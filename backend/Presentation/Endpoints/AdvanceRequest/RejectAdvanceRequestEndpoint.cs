using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Endpoints.AdvanceRequest;

public class RejectAdvanceRequestEndpoint : EndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        var advanceRequests = app.MapGroup("/api/v1/advancerequests")
            .WithTags("Advance Requests")
            .WithOpenApi();

        advanceRequests.MapPost("/{id:guid}/reject", HandleAsync)
            .WithName("RejectAdvanceRequest")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RejectAdvanceRequestCommand(id, DateTime.Today);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: error => error.Code switch
            {
                "AdvanceRequest.NotFound" => Results.NotFound(new ApiError(error.Code, error.Message)),
                "Validation" => Results.BadRequest(new ApiError(error.Code, error.Message)),
                _ => Results.Problem(
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError)
            });
    }
}
