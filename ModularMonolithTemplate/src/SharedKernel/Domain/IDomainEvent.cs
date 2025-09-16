using System;

namespace ModularMonolith.Platform.SharedKernel.Domain;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
