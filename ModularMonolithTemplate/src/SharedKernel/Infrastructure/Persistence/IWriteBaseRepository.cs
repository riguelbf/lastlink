using ModularMonolith.Platform.SharedKernel.Domain;

namespace ModularMonolith.Platform.SharedKernel.Persistence;

public interface IWriteBaseRepository<T> where T : class, IAggregateRoot
{
    Task AddAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task DeleteAsync(T entity, CancellationToken ct);
    Task DeleteByIdAsync(Guid id, CancellationToken ct);
}
