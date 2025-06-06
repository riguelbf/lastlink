namespace SharedKernel.Primitives;

/// <summary>
/// Marker interface for domain events.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the ID of the event.
    /// </summary>
    Guid EventId { get; }
    
    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}
