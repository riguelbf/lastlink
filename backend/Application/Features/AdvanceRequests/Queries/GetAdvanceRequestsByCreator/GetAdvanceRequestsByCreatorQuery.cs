using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestsByCreator;

public sealed record GetAdvanceRequestsByCreatorQuery(Guid CreatorId) 
    : IRequest<Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>>;
