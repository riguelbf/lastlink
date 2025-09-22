using System;

namespace ModularMonolithTemplate.SharedKernel.Domain;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
