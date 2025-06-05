using System.Collections.Immutable;
using Domain.AdvanceRequests.Repositories;
using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequests;

/// <summary>
/// Handles the retrieval of advance requests for a specific creator.
/// </summary>
internal sealed class GetAdvanceRequestsQueryHandler 
    : IRequestHandler<GetAdvanceRequestsQuery, Result<GetAdvanceRequestsResponse>>
{
    private readonly IAdvanceRequestRepository _advanceRequestRepository;

    public GetAdvanceRequestsQueryHandler(IAdvanceRequestRepository advanceRequestRepository)
    {
        _advanceRequestRepository = advanceRequestRepository;
    }

    public async Task<Result<GetAdvanceRequestsResponse>> Handle(
        GetAdvanceRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var requests = await _advanceRequestRepository
            .GetByCreatorIdAsync(request.CreatorId, cancellationToken);

        var requestDtos = requests
            .Select(r => new AdvanceRequestDto(
                r.Id,
                r.CreatorId,
                r.RequestedAmount,
                r.NetAmount,
                r.RequestDate,
                r.Status.ToString(),
                r.ApprovedDate,
                r.RejectedDate))
            .ToImmutableList();

        return new GetAdvanceRequestsResponse(requestDtos);
    }
}
