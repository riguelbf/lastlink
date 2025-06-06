using Application.Features.AdvanceRequests.Queries.GetAdvanceRequestsByCreator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints.AdvanceRequest;

public class GetAdvanceRequestsByCreatorEndpoint : EndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        var advanceRequests = app.MapGroup("/api/v1/advancerequests")
            .WithTags("Advance Requests")
            .WithOpenApi();

        advanceRequests.MapGet("/creator/{creatorId}", HandleAsync )
            .WithName("GetAdvanceRequestsByCreator")
            .Produces<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>().Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] string creatorId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAdvanceRequestsByCreatorQuery(creatorId);
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
