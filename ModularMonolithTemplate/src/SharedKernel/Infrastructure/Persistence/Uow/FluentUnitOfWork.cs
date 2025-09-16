using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Platform.SharedKernel.Domain;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Audit;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.DomainNotifications;

namespace ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Uow;

public sealed class FluentUnitOfWork<TDbContext> where TDbContext : DbContext
{
    private readonly IUnitOfWork _uow;
    private readonly TDbContext _db;

    public FluentUnitOfWork(IUnitOfWork uow, TDbContext db) { _uow = uow; _db = db; }

    // --- minimal config for the example ---
    private TransactionMode _mode = TransactionMode.Auto;
    private IsolationLevel _isolation = IsolationLevel.ReadCommitted;
    private bool _readOnly;
    private bool _readForUpdate;

    public FluentUnitOfWork<TDbContext> WithMode(TransactionMode mode) { _mode = mode; return this; }
    public FluentUnitOfWork<TDbContext> WithIsolation(IsolationLevel lvl) { _isolation = lvl; return this; }
    public FluentUnitOfWork<TDbContext> ForReadOnly() { _readOnly = true; return this; }
    public FluentUnitOfWork<TDbContext> ForReadForUpdate() { _readForUpdate = true; return this; }

    // Enable the combo: Audit + Domain Notifications before commit
    public FluentUnitOfWork<TDbContext> EnableAuditAndDomainNotifications(Func<string> getUserId)
    {
        _onBeforeCommitHooks.Add(async ct =>
        {
            await AppendAuditLogsAsync(getUserId(), ct);
            await PersistDomainNotificationsAsync(ct);
        });
        return this;
    }

    // Hooks
    private readonly List<Func<CancellationToken, Task>> _onBeforeCommitHooks = new();

    public Task RunAsync(Func<TDbContext, CancellationToken, Task> action, CancellationToken ct = default)
        => RunInternalAsync<object?>(async (db, token) => { await action(db, token); return null; }, ct);

    public Task<TResult> RunAsync<TResult>(Func<TDbContext, CancellationToken, Task<TResult>> action, CancellationToken ct = default)
        => RunInternalAsync(action, ct);

    private async Task<TResult> RunInternalAsync<TResult>(Func<TDbContext, CancellationToken, Task<TResult>> action, CancellationToken ct)
    {
        var needTx = ShouldOpenTransaction();

        if (needTx) await _uow.BeginTransactionAsync(_isolation, ct);

        var result = await action(_db, ct);

        if (needTx)
        {
            foreach (var hook in _onBeforeCommitHooks)
                await hook(ct);

            if (_db.ChangeTracker.HasChanges())
                await _uow.SaveChangesAsync(ct);

            await _uow.CommitAsync(ct);
        }
        else if (_db.ChangeTracker.HasChanges())
        {
            await _uow.SaveChangesAsync(ct);
        }

        return result;
    }

    private bool ShouldOpenTransaction()
    {
        if (_mode == TransactionMode.Always) return true;
        if (_mode == TransactionMode.Never) return false;
        if (_readForUpdate) return true;
        if (_readOnly) return false;
        if (_db.ChangeTracker.HasChanges()) return true;
        return false;
    }

    // ---------------- AUDIT ----------------
    private async Task AppendAuditLogsAsync(string userId, CancellationToken ct)
    {
        var entries = _db.ChangeTracker.Entries()
            .Where(e => e.Entity is not AuditLog &&
                        e.Entity is not DomainNotification &&
                        e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var e in entries)
        {
            var entityName = e.Entity.GetType().Name;
            var idProp = e.Properties.FirstOrDefault(p => string.Equals(p.Metadata.Name, "Id", StringComparison.OrdinalIgnoreCase));
            var entityId = idProp?.CurrentValue?.ToString() ?? "(no-id)";

            var snapshot = e.State switch
            {
                EntityState.Added    => e.CurrentValues.ToObject(),
                EntityState.Modified => e.CurrentValues.ToObject(),
                EntityState.Deleted  => e.OriginalValues.ToObject(),
                _ => null
            };

            var log = new AuditLog
            {
                TimestampUtc = DateTime.UtcNow,
                UserId = userId,
                EntityName = entityName,
                EntityId = entityId,
                Operation = e.State.ToString(),
                DataJson = JsonSerializer.Serialize(snapshot, Infrastructure.Persistence.AppDbContext.SafeJson)
            };

            await _db.Set<AuditLog>().AddAsync(log, ct);
        }
    }

    // --------------- DOMAIN NOTIFICATIONS ---------------
    private async Task PersistDomainNotificationsAsync(CancellationToken ct)
    {
        var aggregates = _db.ChangeTracker.Entries()
            .Where(e => e.Entity is AggregateRoot)
            .Select(e => (AggregateRoot)e.Entity)
            .ToList();

        foreach (var agg in aggregates)
        {
            foreach (var ev in agg.DomainEvents)
            {
                var dn = new DomainNotification
                {
                    EventType = ev.GetType().FullName ?? ev.GetType().Name,
                    EventJson = JsonSerializer.Serialize(ev, Infrastructure.Persistence.AppDbContext.SafeJson),
                    OccurredAtUtc = (ev is IDomainEvent de) ? de.OccurredOnUtc : DateTime.UtcNow,
                    CreatedAtUtc = DateTime.UtcNow,
                    AggregateId = TryGetAggregateId(agg)
                };
                await _db.Set<DomainNotification>().AddAsync(dn, ct);
            }

            // clear to avoid duplicates on replays
            agg.ClearDomainEvents();
        }
    }

    private static Guid TryGetAggregateId(AggregateRoot agg)
    {
        var prop = agg.GetType().GetProperty("Id");
        if (prop?.GetValue(agg) is Guid g) return g;
        return Guid.Empty;
    }
}

public static class FluentUnitOfWorkExtensions
{
    public static FluentUnitOfWork<TDbContext> Fluent<TDbContext>(this IUnitOfWork uow, TDbContext db)
        where TDbContext : DbContext => new(uow, db);
}
