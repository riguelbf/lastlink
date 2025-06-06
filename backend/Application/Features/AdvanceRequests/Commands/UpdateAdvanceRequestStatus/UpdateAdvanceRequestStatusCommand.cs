using MediatR;
using SharedKernel.Primitives;

namespace Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;

/// <summary>
/// Base class for update status commands.
/// </summary>
public abstract record UpdateAdvanceRequestStatusCommand(
    Guid RequestId,
    DateTime ActionDate) : IRequest<Result>;

/// <summary>
/// Command to approve an advance request.
/// </summary>
public sealed record ApproveAdvanceRequestCommand(
    Guid RequestId,
    DateTime ActionDate) 
    : UpdateAdvanceRequestStatusCommand(RequestId, ActionDate);

/// <summary>
/// Command to reject an advance request.
/// </summary>
public sealed record RejectAdvanceRequestCommand(
    Guid RequestId,
    DateTime ActionDate) 
    : UpdateAdvanceRequestStatusCommand(RequestId, ActionDate);
