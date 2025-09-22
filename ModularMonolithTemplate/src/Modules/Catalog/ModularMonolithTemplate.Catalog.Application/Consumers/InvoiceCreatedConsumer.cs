using System.Text.Json;
using ModularMonolithTemplate.Catalog.Infrastructure.Persistence;
using ModularMonolithTemplate.Billing.Domain.Aggregates.Invoice.Events;
using ModularMonolithTemplate.Billing.Domain.Projections;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;
using ModularMonolithTemplate.SharedKernel.Messaging;

namespace ModularMonolithTemplate.Catalog.Application.Consumers;

public sealed class InvoiceCreatedConsumer(CatalogReadDbContext readDb) : IEventConsumer
{
    public async Task HandleAsync(string eventType, string eventJson, CancellationToken ct = default)
    {
        // Handle only Billing.InvoiceCreated
        if (eventType != typeof(InvoiceCreated).FullName)
            return;

        var ev = JsonSerializer.Deserialize<InvoiceCreated>(eventJson, AppDbContext.SafeJson);
        if (ev is null) return;

        var doc = new CatalogInvoice
        {
            BillingInvoiceId = ev.InvoiceId,
            CustomerId = ev.CustomerId,
            Amount = ev.Amount,
            CreatedAtUtc = ev.OccurredOnUtc
        };

        await readDb.Invoices.AddAsync(doc, ct);
        await readDb.SaveChangesAsync(ct);
    }
}
