using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Uow;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(IsolationLevel isolation, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
}
