using System;

namespace SharedKernel.Primitives;

/// <summary>
/// Base class for aggregate roots in the domain model.
/// </summary>
public abstract class AggregateRoot : EntityBase
{
    /// <summary>
    /// Gets the aggregate root identifier.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
    /// </summary>
    protected AggregateRoot()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class with the specified ID.
    /// </summary>
    /// <param name="id">The aggregate root ID.</param>
    protected AggregateRoot(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("ID cannot be empty", nameof(id));
        }

        Id = id;
    }
}
