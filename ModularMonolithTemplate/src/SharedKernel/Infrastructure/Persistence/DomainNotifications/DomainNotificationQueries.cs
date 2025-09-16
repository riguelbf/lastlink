using Microsoft.EntityFrameworkCore;

namespace ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.DomainNotifications;

public static class DomainNotificationQueries
{
    // Leases a batch of unprocessed notifications in a cooperative way
    public static async Task<List<DomainNotification>> LeaseBatchAsync(DbContext db, int batchSize, CancellationToken ct)
    {
        return await db.Set<DomainNotification>()
            .Where(n => !n.Processed)
            .OrderBy(n => n.Id)
            .Take(batchSize)
            .ToListAsync(ct);
    }
}
