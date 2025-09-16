using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Platform.SharedKernel.Messaging;

public sealed class InProcessEventBus(IServiceProvider serviceProvider) : IEventBus
{
    public async Task PublishAsync(string eventType, string eventJson, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var consumers = scope.ServiceProvider.GetServices<IEventConsumer>();
        foreach (var consumer in consumers)
        {
            try
            {
                await consumer.HandleAsync(eventType, eventJson, ct);
            }
            catch
            {
                // let caller handle retries and errors (publisher does)
                throw;
            }
        }
    }
}
