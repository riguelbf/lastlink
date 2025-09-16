using System;
using ModularMonolith.Platform.SharedKernel.Domain;
using WebApp.Modules.Billing.Domain.Events;

namespace WebApp.Modules.Billing.Domain;

public sealed class Invoice : AggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string CustomerId { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Invoice() { }

    public static Invoice Create(string customerId, decimal amount)
    {
        var invoice = new Invoice
        {
            CustomerId = customerId,
            Amount = amount,
            CreatedAtUtc = DateTime.UtcNow
        };

        invoice.AddDomainEvent(new InvoiceCreated(invoice.Id, invoice.CustomerId, invoice.Amount, DateTime.UtcNow));
        return invoice;
    }
}
