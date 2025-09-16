using System.Text.Json;
using ModularMonolith.Platform.SharedKernel.Messaging;
using WebApp.Modules.Catalog.Infra;
using WebApp.Modules.Catalog.Domain.Projections;
using WebApp.Modules.Billing.Domain.Events;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;

namespace WebApp.Modules.Catalog.Application.Consumers;

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
