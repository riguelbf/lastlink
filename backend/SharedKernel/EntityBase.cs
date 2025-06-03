namespace SharedKernel;

public abstract class EntityBase
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public bool IsDeleted { get; set; } = false;

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}