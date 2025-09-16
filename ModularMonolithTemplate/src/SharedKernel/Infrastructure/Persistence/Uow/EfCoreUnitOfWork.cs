using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Uow;

public sealed class EfCoreUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _db;
    private IDbContextTransaction? _tx;

    public EfCoreUnitOfWork(TDbContext db) => _db = db;

    public async Task BeginTransactionAsync(IsolationLevel isolation, CancellationToken ct = default)
    {
        if (_tx is not null) return;
        // Use provider-agnostic overload to support InMemory provider
        // (some providers don't support the IsolationLevel overload)
        _tx = await _db.Database.BeginTransactionAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_tx is null) return;
        await _tx.CommitAsync(ct);
        await _tx.DisposeAsync();
        _tx = null;
    }
}
