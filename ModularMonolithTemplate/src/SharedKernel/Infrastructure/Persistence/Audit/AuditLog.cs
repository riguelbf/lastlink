using System;

namespace ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Audit;

public sealed class AuditLog
{
    public long Id { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; }
    public string EntityName { get; set; } = default!;
    public string EntityId { get; set; } = default!;
    public string Operation { get; set; } = default!; // Added/Modified/Deleted
    public string DataJson { get; set; } = "{}";
}
