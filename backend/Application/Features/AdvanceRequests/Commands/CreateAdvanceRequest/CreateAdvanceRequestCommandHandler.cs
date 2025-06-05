using Domain.AdvanceRequests;
using Domain.AdvanceRequests.Repositories;
using MediatR;
using SharedKernel.Primitives;
using IUnitOfWork = Infrastructure.DataBase.Repositories.Base.IUnitOfWork;

namespace Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;

/// <summary>
/// Handles the creation of a new advance request.
/// </summary>
internal sealed class CreateAdvanceRequestCommandHandler : IRequestHandler<CreateAdvanceRequestCommand, Result<CreateAdvanceRequestResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAdvanceRequestCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateAdvanceRequestResponse>> Handle(
        CreateAdvanceRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Check if creator already has a pending request
        var advanceRequestRepository = _unitOfWork.GetRepository<IAdvanceRequestRepository>();
        var hasPendingRequest = await advanceRequestRepository
            .HasPendingRequestAsync(request.CreatorId, cancellationToken);
             
        if (hasPendingRequest)
        {
            return Result.Failure<CreateAdvanceRequestResponse>(
                Error.Validation(
                    "AdvanceRequest.PendingRequestExists",
                    "You already have a pending advance request."));
        }

        var advanceRequestResult = AdvanceRequest.Create(
            request.CreatorId,
            request.RequestedAmount,
            request.RequestDate);

        if (advanceRequestResult.IsFailure)
        {
            return Result.Failure<CreateAdvanceRequestResponse>(advanceRequestResult.Error);
        }

        var advanceRequest = advanceRequestResult.Value;

        await advanceRequestRepository.AddAsync(advanceRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateAdvanceRequestResponse(
            advanceRequest.Id,
            advanceRequest.CreatorId,
            advanceRequest.RequestedAmount,
            advanceRequest.NetAmount,
            advanceRequest.RequestDate,
            advanceRequest.Status.ToString());

        return response;
    }
}
