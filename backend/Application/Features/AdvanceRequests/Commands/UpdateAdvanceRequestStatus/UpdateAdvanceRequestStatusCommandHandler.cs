using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Domain.AdvanceRequests.Repositories;
using MediatR;
using SharedKernel.Primitives;
using IUnitOfWork = Infrastructure.DataBase.Repositories.Base.IUnitOfWork;

namespace Application.Features.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;

/// <summary>
/// Handles updating the status of an advance request.
/// </summary>
internal sealed class UpdateAdvanceRequestStatusCommandHandler : 
    IRequestHandler<ApproveAdvanceRequestCommand, Result>,
    IRequestHandler<RejectAdvanceRequestCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAdvanceRequestStatusCommandHandler(
        IUnitOfWork unitOfWork)
    {
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
        var request = await _unitOfWork
            .GetRepository<IAdvanceRequestRepository>()
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
