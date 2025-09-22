using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolithTemplate.SharedKernel.Messaging
{
    public sealed class InProcessEventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public InProcessEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync(string eventType, string eventJson, CancellationToken ct = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var consumers = scope.ServiceProvider.GetServices<IEventConsumer>();
            foreach (var c in consumers)
            {
                await c.HandleAsync(eventType, eventJson, ct);
            }
        }
    }
}
