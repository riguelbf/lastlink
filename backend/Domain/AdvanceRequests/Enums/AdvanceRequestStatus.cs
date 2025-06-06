namespace Domain.AdvanceRequests.Enums;

/// <summary>
/// Represents the possible statuses of an advance request.
/// </summary>
public enum AdvanceRequestStatus
{
    /// <summary>
    /// The request is pending approval.
    /// </summary>
    Pending,
    
    /// <summary>
    /// The request has been approved.
    /// </summary>
    Approved,
    
    /// <summary>
    /// The request has been rejected.
    /// </summary>
    Rejected
}
