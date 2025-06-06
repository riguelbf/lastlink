namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequests;

/// <summary>
/// Response containing a list of advance requests for a creator.
/// </summary>
public sealed record GetAdvanceRequestsResponse(
    IReadOnlyList<AdvanceRequestDto> Requests);

/// <summary>
/// DTO representing an advance request in the response.
/// </summary>
public sealed record AdvanceRequestDto(
    Guid Id,
    string CreatorId,
    decimal RequestedAmount,
    decimal NetAmount,
    DateTime RequestDate,
    string Status,
    DateTime? ApprovedDate,
    DateTime? RejectedDate);
