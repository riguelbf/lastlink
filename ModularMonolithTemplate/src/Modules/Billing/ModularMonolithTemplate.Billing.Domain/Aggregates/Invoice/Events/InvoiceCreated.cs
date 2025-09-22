using ModularMonolithTemplate.SharedKernel.Domain;

namespace ModularMonolithTemplate.Billing.Domain.Aggregates.Invoice.Events;

public sealed record InvoiceCreated(Guid InvoiceId, string CustomerId, decimal Amount, DateTime OccurredOnUtc) : IDomainEvent;
