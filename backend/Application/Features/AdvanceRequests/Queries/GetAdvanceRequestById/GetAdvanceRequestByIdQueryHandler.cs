using Domain.AdvanceRequests.Repositories;
using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;

internal sealed class GetAdvanceRequestByIdQueryHandler(IAdvanceRequestRepository advanceRequestRepository)
    : IRequestHandler<GetAdvanceRequestByIdQuery, Result<GetAdvanceRequestByIdResponse>>
{
    public async Task<Result<GetAdvanceRequestByIdResponse>> Handle(
        GetAdvanceRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var advanceRequest = await advanceRequestRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (advanceRequest is null)
        {
            return Result.Failure<GetAdvanceRequestByIdResponse>(
                Error.NotFound("AdvanceRequest.NotFound", "The specified advance request was not found."));
        }

        var response = new GetAdvanceRequestByIdResponse(
            advanceRequest.Id,
            advanceRequest.RequestedAmount,
            advanceRequest.Status.ToString(),
            advanceRequest.RequestDate,
            advanceRequest.CreatorId);

        return response;
    }
}
