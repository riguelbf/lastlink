using System;

namespace ModularMonolithTemplate.SharedKernel.Domain;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
