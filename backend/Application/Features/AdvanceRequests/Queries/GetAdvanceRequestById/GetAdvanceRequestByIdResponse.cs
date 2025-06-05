namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;

public sealed record GetAdvanceRequestByIdResponse(
    Guid Id,
    decimal Amount,
    string Status,
    DateTime RequestDate,
    string RequesterId);
