using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;

/// <summary>
/// Command to create a new advance request.
/// </summary>
public sealed record CreateAdvanceRequestCommand(
    string CreatorId,
    decimal RequestedAmount,
    DateTime RequestDate) : IRequest<Result<CreateAdvanceRequestResponse>>;
