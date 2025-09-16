using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Platform.SharedKernel.Messaging;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;

namespace ModularMonolith.Platform.SharedKernel.Domain;

public sealed class DomainEventDispatcher(IEventBus bus)
{
    public async Task DispatchAsync(DbContext db, CancellationToken ct)
    {
        var aggregates = db.ChangeTracker.Entries()
            .Where(e => e.Entity is AggregateRoot)
            .Select(e => (AggregateRoot)e.Entity)
            .ToList();

        foreach (var agg in aggregates)
        {
            foreach (var ev in agg.DomainEvents)
            {
                var type = ev.GetType().FullName ?? ev.GetType().Name;
                var json = JsonSerializer.Serialize(ev, AppDbContext.SafeJson);
                await bus.PublishAsync(type, json, ct);
            }

            agg.ClearDomainEvents();
        }
    }
}
