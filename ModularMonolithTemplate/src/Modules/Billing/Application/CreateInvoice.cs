using System.Diagnostics;
using MediatR;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Uow;
using Modules.Billing.Infra;
using WebApp.Modules.Billing.Domain;
using WebApp.Modules.Billing.Infra;

namespace Modules.Billing.Application;

public sealed record CreateInvoiceCommand(string CustomerId, decimal Amount) : IRequest<Guid>;

public sealed class CreateInvoiceHandler(
    FluentUnitOfWork<BillingWriteDbContext> fUow,
    BillingWriteDbContext db
) : IRequestHandler<CreateInvoiceCommand, Guid>
{
    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken ct)
    {
        using var act = WebApp.Modules.Billing.Observability.Activity.StartActivity("Billing.CreateInvoice", ActivityKind.Internal);
        act?.SetTag("bc", "Billing");
        act?.SetTag("slice", "Invoices/Create");
        act?.SetTag("customer.id", request.CustomerId);
        act?.SetTag("amount", request.Amount);

        var invoice = Invoice.Create(request.CustomerId, request.Amount);

        await fUow
            .EnableAuditAndDomainNotifications(() => request.CustomerId)
            .RunAsync(async (ctx, token) =>
            {
                await db.Invoices.AddAsync(invoice, token);
            }, ct);

        return invoice.Id;
    }
}
