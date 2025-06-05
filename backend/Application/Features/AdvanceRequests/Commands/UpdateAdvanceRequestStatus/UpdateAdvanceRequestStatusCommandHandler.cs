using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Domain.AdvanceRequests.Repositories;
using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;

/// <summary>
/// Handles updating the status of an advance request.
/// </summary>
internal sealed class UpdateAdvanceRequestStatusCommandHandler : 
    IRequestHandler<ApproveAdvanceRequestCommand, Result>,
    IRequestHandler<RejectAdvanceRequestCommand, Result>
{
    private readonly IAdvanceRequestRepository _advanceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAdvanceRequestStatusCommandHandler(
        IAdvanceRequestRepository advanceRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _advanceRequestRepository = advanceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ApproveAdvanceRequestCommand request,
        CancellationToken cancellationToken)
    {
        return await UpdateRequestStatus(
            request.RequestId, 
            r => r.Approve(request.ActionDate),
            cancellationToken);
    }

    public async Task<Result> Handle(
        RejectAdvanceRequestCommand request,
        CancellationToken cancellationToken)
    {
        return await UpdateRequestStatus(
            request.RequestId, 
            r => r.Reject(request.ActionDate),
            cancellationToken);
    }

    private async Task<Result> UpdateRequestStatus(
        Guid requestId,
        Func<Domain.AdvanceRequests.AdvanceRequest, Result> updateAction,
        CancellationToken cancellationToken)
    {
        var request = await _advanceRequestRepository
            .GetByIdAsync(requestId, cancellationToken);

        if (request is null)
        {
            return Result.Failure<Result>(
                Error.NotFound(
                    "AdvanceRequest.NotFound",
                    "The specified advance request was not found."));
        }

        var result = updateAction(request);
        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
