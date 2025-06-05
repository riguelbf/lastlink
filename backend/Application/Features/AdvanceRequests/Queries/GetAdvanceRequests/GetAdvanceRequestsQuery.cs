using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequests;

/// <summary>
/// Query to get all advance requests for a specific creator.
/// </summary>
public sealed record GetAdvanceRequestsQuery(
    string CreatorId) : IRequest<Result<GetAdvanceRequestsResponse>>;
