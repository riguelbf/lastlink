using System;
using ModularMonolith.Platform.SharedKernel.Domain;

namespace WebApp.Modules.Billing.Domain.Events;

public sealed record InvoiceCreated(Guid InvoiceId, string CustomerId, decimal Amount, DateTime OccurredOnUtc) : IDomainEvent;
