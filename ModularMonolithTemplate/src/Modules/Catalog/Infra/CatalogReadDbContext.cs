using Microsoft.EntityFrameworkCore;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;

namespace WebApp.Modules.Catalog.Infra;

public class CatalogReadDbContext(DbContextOptions<CatalogReadDbContext> options) : AppDbContext(options)
{
    public DbSet<WebApp.Modules.Catalog.Domain.Projections.CatalogInvoice> Invoices => Set<WebApp.Modules.Catalog.Domain.Projections.CatalogInvoice>();
}
