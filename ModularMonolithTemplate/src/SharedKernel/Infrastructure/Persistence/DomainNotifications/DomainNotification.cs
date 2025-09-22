namespace ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.DomainNotifications;

public sealed class DomainNotification
{
    public long Id { get; set; }
    public string EventType { get; set; } = default!;
    public string EventJson { get; set; } = "{}";
    public DateTime OccurredAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Guid AggregateId { get; set; }

    public bool Processed { get; set; } = false;
    public DateTime? ProcessedAtUtc { get; set; }

    // failure tracking
    public int Attempts { get; set; } = 0;
    public string? LastError { get; set; }
}
