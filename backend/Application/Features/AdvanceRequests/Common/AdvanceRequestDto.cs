namespace Application.Features.AdvanceRequests.Common;

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
