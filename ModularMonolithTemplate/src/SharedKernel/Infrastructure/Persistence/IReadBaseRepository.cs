using ModularMonolithTemplate.SharedKernel.Domain;
using ModularMonolithTemplate.SharedKernel.Pagination;

namespace ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;

public interface IReadBaseRepository<T> where T : class, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task<PageResult<T>> GetAllAsync(PageRequest page, CancellationToken ct);
}
