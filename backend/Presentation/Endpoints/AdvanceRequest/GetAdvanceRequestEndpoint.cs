using Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints.AdvanceRequest;

public class GetAdvanceRequestEndpoint : EndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        var advanceRequests = app.MapGroup("/api/v1/advancerequests")
            .WithTags("Advance Requests")
            .WithOpenApi();

        advanceRequests.MapGet("/{id:guid}", HandleAsync)
            .WithName("GetAdvanceRequestById")
            .Produces<GetAdvanceRequestByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAdvanceRequestByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            onSuccess: Results.Ok,
            onFailure: error => error.Code switch
            {
                "NotFound" => Results.NotFound(new { error.Code, error.Message }),
                _ => Results.Problem(
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError)
            });
    }
}
