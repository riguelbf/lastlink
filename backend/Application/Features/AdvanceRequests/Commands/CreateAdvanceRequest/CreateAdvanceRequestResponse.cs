namespace Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;

/// <summary>
/// Response for creating an advance request.
/// </summary>
public sealed record CreateAdvanceRequestResponse(
    Guid Id,
    string CreatorId,
    decimal RequestedAmount,
    decimal NetAmount,
    DateTime RequestDate,
    string Status);
