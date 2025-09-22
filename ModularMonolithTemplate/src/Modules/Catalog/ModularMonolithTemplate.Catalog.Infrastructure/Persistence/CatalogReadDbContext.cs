using Microsoft.EntityFrameworkCore;
using ModularMonolithTemplate.Billing.Domain.Projections;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;

namespace ModularMonolithTemplate.Catalog.Infrastructure.Persistence;

public class CatalogReadDbContext(DbContextOptions<CatalogReadDbContext> options) : AppDbContext(options)
{
    public DbSet<CatalogInvoice> Invoices => Set<CatalogInvoice>();
}
