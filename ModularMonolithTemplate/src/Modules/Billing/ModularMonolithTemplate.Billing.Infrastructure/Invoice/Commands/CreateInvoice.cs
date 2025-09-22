using System.Diagnostics;
using MediatR;
using ModularMonolithTemplate.Billing.Infrastructure.Monitoring;
using ModularMonolithTemplate.Billing.Infrastructure.Persistence;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.Uow;
using InvoiceAgg = ModularMonolithTemplate.Billing.Domain.Aggregates.Invoice.Invoice;

namespace ModularMonolithTemplate.Billing.Infrastructure.Invoice.Commands;

public sealed record CreateInvoiceCommand(string CustomerId, decimal Amount) : IRequest<Guid>;

public sealed class CreateInvoiceHandler(
    FluentUnitOfWork<BillingWriteDbContext> fUow,
    BillingWriteDbContext db
) : IRequestHandler<CreateInvoiceCommand, Guid>
{
    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken ct)
    {
        using var act = Observability.Activity.StartActivity("Billing.CreateInvoice", ActivityKind.Internal);
        act?.SetTag("bc", "Billing");
        act?.SetTag("slice", "Invoices/Create");
        act?.SetTag("customer.id", request.CustomerId);
        act?.SetTag("amount", request.Amount);

        var invoice = InvoiceAgg.Create(request.CustomerId, request.Amount);

        await fUow
            .EnableAuditAndDomainNotifications(() => request.CustomerId)
            .RunAsync(async (ctx, token) =>
            {
                await db.Invoices.AddAsync(invoice, token);
            }, ct);

        return invoice.Id;
    }
}
