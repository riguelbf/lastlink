using ModularMonolith.Platform.SharedKernel.Domain;
using ModularMonolith.Platform.SharedKernel.Pagination;

namespace ModularMonolith.Platform.SharedKernel.Persistence;

public interface IReadBaseRepository<T> where T : class, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task<PageResult<T>> GetAllAsync(PageRequest page, CancellationToken ct);
}
