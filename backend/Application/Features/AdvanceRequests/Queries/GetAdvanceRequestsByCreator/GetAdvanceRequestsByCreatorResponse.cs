namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestsByCreator;

public sealed record GetAdvanceRequestsByCreatorResponse(
    Guid Id,
    decimal Amount,
    string Status,
    DateTime RequestDate,
    string RequesterId);
