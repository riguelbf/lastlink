namespace ModularMonolith.Platform.SharedKernel.Messaging;

public interface IEventBus
{
    Task PublishAsync(string eventType, string eventJson, CancellationToken ct = default);
}

public interface IEventConsumer
{
    Task HandleAsync(string eventType, string eventJson, CancellationToken ct = default);
}
