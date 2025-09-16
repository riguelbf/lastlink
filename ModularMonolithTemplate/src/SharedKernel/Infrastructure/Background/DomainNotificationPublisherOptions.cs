namespace ModularMonolith.Platform.SharedKernel.Infrastructure.Background;

public sealed class DomainNotificationPublisherOptions
{
    public int BatchSize { get; set; } = 50;
    public int MaxImmediateRetries { get; set; } = 3;
    public int MaxAttempts { get; set; } = 10;
    public TimeSpan FirstBackoff { get; set; } = TimeSpan.FromMilliseconds(100);
    public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(2);
}
