using Domain.AdvanceRequests.Repositories;
using Infrastructure.DataBase.Repositories.Base;
using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestsByCreator;

internal sealed class GetAdvanceRequestsByCreatorQueryHandler(IAdvanceRequestRepository advanceRequestRepository)
    : IRequestHandler<GetAdvanceRequestsByCreatorQuery, Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>> Handle(
        GetAdvanceRequestsByCreatorQuery request,
        CancellationToken cancellationToken)
    {
        var requests = await advanceRequestRepository
            .GetByCreatorIdAsync(request.CreatorId.ToString(), cancellationToken);

        var response = requests
            .Select(r => new GetAdvanceRequestsByCreatorResponse(
                r.Id,
                r.RequestedAmount,
                r.Status.ToString(),
                r.RequestDate,
                r.CreatorId))
            .ToList()
            .AsReadOnly();

        return response;
    }
}
