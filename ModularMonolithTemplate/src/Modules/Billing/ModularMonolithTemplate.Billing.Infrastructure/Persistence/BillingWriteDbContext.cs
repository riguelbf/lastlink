using Microsoft.EntityFrameworkCore;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.DomainNotifications;

namespace ModularMonolithTemplate.Billing.Infrastructure.Persistence;

public class BillingWriteDbContext(DbContextOptions<BillingWriteDbContext> options) : AppDbContext(options)
{
    public DbSet<Domain.Aggregates.Invoice.Invoice> Invoices => Set<Domain.Aggregates.Invoice.Invoice>();
    public DbSet<DomainNotification> DomainNotifications => Set<DomainNotification>();
}
