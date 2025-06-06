using SharedKernel.Primitives;

namespace Domain.AdvanceRequests.Repositories;

/// <summary>
/// Defines the contract for the advance request repository.
/// </summary>
public interface IAdvanceRequestRepository : IRepository<AdvanceRequest>
{
    /// <summary>
    /// Adds a new advance request to the repository.
    /// </summary>
    /// <param name="advanceRequest">The advance request to add.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(AdvanceRequest advanceRequest, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves an advance request by its ID.
    /// </summary>
    /// <param name="id">The ID of the advance request to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the advance request if found; otherwise, null.</returns>
    Task<AdvanceRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all advance requests for a specific creator.
    /// </summary>
    /// <param name="creatorId">The ID of the creator.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of advance requests.</returns>
    Task<List<AdvanceRequest>> GetByCreatorIdAsync(string creatorId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if a creator has any pending advance requests.
    /// </summary>
    /// <param name="creatorId">The ID of the creator to check.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the creator has any pending requests; otherwise, false.</returns>
    Task<bool> HasPendingRequestAsync(string creatorId, CancellationToken cancellationToken = default);
}
