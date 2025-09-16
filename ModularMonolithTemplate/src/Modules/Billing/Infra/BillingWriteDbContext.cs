using Microsoft.EntityFrameworkCore;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.DomainNotifications;

namespace Modules.Billing.Infra;

public class BillingWriteDbContext(DbContextOptions<BillingWriteDbContext> options) : AppDbContext(options)
{
    public DbSet<WebApp.Modules.Billing.Domain.Invoice> Invoices => Set<WebApp.Modules.Billing.Domain.Invoice>();
    public DbSet<DomainNotification> DomainNotifications => Set<DomainNotification>();
}
