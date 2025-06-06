using System;
using System.Collections.Generic;

namespace SharedKernel.Primitives;

/// <summary>
/// Base class for entities in the domain model.
/// </summary>
public abstract class EntityBase
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    /// <summary>
    /// Gets a value indicating whether the entity has been marked as deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets the domain events raised by this entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events raised by this entity.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Raises a domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent ?? throw new ArgumentNullException(nameof(domainEvent)));
    }
}
