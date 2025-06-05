namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;

public sealed record GetAdvanceRequestByIdResponse(
    Guid Id,
    decimal Amount,
    string Status,
    string Description,
    DateTime RequestDate,
    string RequesterId);
