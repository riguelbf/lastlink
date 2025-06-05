using Domain.AdvanceRequests;
using Domain.AdvanceRequests.Repositories;
using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;

/// <summary>
/// Handles the creation of a new advance request.
/// </summary>
internal sealed class CreateAdvanceRequestCommandHandler : IRequestHandler<CreateAdvanceRequestCommand, Result<CreateAdvanceRequestResponse>>
{
    private readonly IAdvanceRequestRepository _advanceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAdvanceRequestCommandHandler(
        IAdvanceRequestRepository advanceRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _advanceRequestRepository = advanceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateAdvanceRequestResponse>> Handle(
        CreateAdvanceRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Check if creator already has a pending request
        var hasPendingRequest = await _advanceRequestRepository
            .HasPendingRequestAsync(request.CreatorId, cancellationToken);
             
        if (hasPendingRequest)
        {
            return Result.Failure<CreateAdvanceRequestResponse>(
                Error.Validation(
                    "AdvanceRequest.PendingRequestExists",
                    "You already have a pending advance request."));
        }

        // Create the advance request
        var advanceRequestResult = AdvanceRequest.Create(
            request.CreatorId,
            request.RequestedAmount,
            request.RequestDate);

        if (advanceRequestResult.IsFailure)
        {
            return Result.Failure<CreateAdvanceRequestResponse>(advanceRequestResult.Error);
        }

        var advanceRequest = advanceRequestResult.Value;

        // Add to repository
        await _advanceRequestRepository.AddAsync(advanceRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return response
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
