using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Background;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.DomainNotifications;
using ModularMonolithTemplate.SharedKernel.Messaging;

namespace ModularMonolithTemplate.SharedKernel.Infrastructure.Background;

public sealed class DomainNotificationPublisher(
    IServiceScopeFactory scopeFactory,
    IOptions<DomainNotificationPublisherOptions> optionsAccessor,
    ILogger<DomainNotificationPublisher> logger
) : BackgroundService
{
    private readonly DomainNotificationPublisherOptions options = optionsAccessor.Value;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(options.PollInterval);
        logger.LogInformation("DomainNotificationPublisher started (batch={Batch}, interval={Interval})",
            options.BatchSize, options.PollInterval);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await TickAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error in DomainNotificationPublisher");
        }
    }

    private async Task TickAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        var batch = await DomainNotificationQueries.LeaseBatchAsync(db, options.BatchSize, ct);
        if (batch.Count == 0) return;

        foreach (var n in batch)
        {
            try
            {
                await PublishWithImmediateRetryAsync(bus, n, ct);

                n.Processed = true;
                n.ProcessedAtUtc = DateTime.UtcNow;
                n.Attempts++;
                n.LastError = null;

                await db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                n.Attempts++;
                n.LastError = ex.Message;
                await db.SaveChangesAsync(ct);

                if (n.Attempts >= options.MaxAttempts)
                {
                    logger.LogError(ex, "DomainNotification {Id} moved to poison (attempts={Attempts})", n.Id, n.Attempts);
                }
            }
        }
    }

    private async Task PublishWithImmediateRetryAsync(IEventBus bus, DomainNotification n, CancellationToken ct)
    {
        var attempt = 0;
        var delay = options.FirstBackoff;

        while (true)
        {
            try
            {
                await bus.PublishAsync(n.EventType, n.EventJson, ct);
                return;
            }
            catch when (attempt < options.MaxImmediateRetries)
            {
                attempt++;
                await Task.Delay(Jitter(delay, attempt), ct);
                continue;
            }
        }
    }

    private static TimeSpan Jitter(TimeSpan baseDelay, int attempt)
    {
        var r = Random.Shared.NextDouble(); // 0..1
        return TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * (0.5 + r) * Math.Pow(2, attempt));
    }
}
