using Domain.AdvanceRequests.Enums;
using Domain.AdvanceRequests.ValueObjects;
using SharedKernel.Primitives;

namespace Domain.AdvanceRequests;

/// <summary>
/// Represents a request from a creator to receive an advance on their future earnings.
/// </summary>
public class AdvanceRequest : AggregateRoot, IAuditableEntity
{
    private IAuditableEntity _auditableEntityImplementation;

    private AdvanceRequest()
    {
        // For EF Core
    }

    private AdvanceRequest(
        Guid id,
        string creatorId,
        decimal requestedAmount,
        decimal netAmount,
        DateTime requestDate,
        AdvanceRequestStatus status) : base(id)
    {
        CreatorId = creatorId;
        RequestedAmount = requestedAmount;
        NetAmount = netAmount;
        RequestDate = requestDate;
        Status = status;
    }

    public string CreatorId { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    public DateTime RequestDate { get; private set; }
    public AdvanceRequestStatus Status { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public DateTime? RejectedDate { get; private set; }
    public DateTime CreatedAt { get; set; }
    
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Creates a new advance request.
    /// </summary>
    /// <param name="creatorId">The ID of the creator making the request.</param>
    /// <param name="requestedAmount">The amount being requested.</param>
    /// <param name="requestDate">The date of the request.</param>
    /// <returns>A result containing the new advance request or an error.</returns>
    public static Result<AdvanceRequest> Create(
        string creatorId,
        decimal requestedAmount,
        DateTime requestDate)
    {
        var amountResult = Amount.Create(requestedAmount);
        if (amountResult.IsFailure)
        {
            return Result.Failure<AdvanceRequest>(Error.Validation("Amount.Invalid", amountResult.Error!.Message));
        }

        var amount = amountResult.Value;
        var netAmount = amount.CalculateNetAmount();

        var utcRequestDate = requestDate.Kind == DateTimeKind.Utc 
            ? requestDate 
            : requestDate.ToUniversalTime();

        var request = new AdvanceRequest(
            Guid.NewGuid(),
            creatorId,
            amount.Value,
            netAmount,
            utcRequestDate,
            AdvanceRequestStatus.Pending)
        {
            CreatedAt = DateTime.UtcNow
        };

        // TODO: Add domain event for request creation

        return request;
    }

    /// <summary>
    /// Approves the advance request.
    /// </summary>
    /// <param name="approvalDate">The date of approval.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result Approve(DateTime approvalDate)
    {
        if (Status != AdvanceRequestStatus.Pending)
        {
            return Result.Failure<Result>(Error.Validation(
                "AdvanceRequest.InvalidStatus",
                "Only pending requests can be approved."));
        }

        Status = AdvanceRequestStatus.Approved;
        ApprovedDate = approvalDate.Kind == DateTimeKind.Utc ? approvalDate : approvalDate.ToUniversalTime();
        ModifiedAt = DateTime.UtcNow;

        // TODO: Add domain event for approval

        return Result.Success();
    }

    /// <summary>
    /// Rejects the advance request.
    /// </summary>
    /// <param name="rejectionDate">The date of rejection.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result Reject(DateTime rejectionDate)
    {
        if (Status != AdvanceRequestStatus.Pending)
        {
            return Result.Failure<Result>(Error.Validation(
                "AdvanceRequest.InvalidStatus",
                "Only pending requests can be rejected."));
        }

        Status = AdvanceRequestStatus.Rejected;
        RejectedDate = rejectionDate.Kind == DateTimeKind.Utc ? rejectionDate : rejectionDate.ToUniversalTime();
        ModifiedAt = DateTime.UtcNow;

        // TODO: Add domain event for rejection

        return Result.Success();
    }
}
