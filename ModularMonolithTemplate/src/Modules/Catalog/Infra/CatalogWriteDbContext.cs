using Microsoft.EntityFrameworkCore;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;

namespace WebApp.Modules.Catalog.Infra;

public class CatalogWriteDbContext(DbContextOptions<CatalogWriteDbContext> options) : AppDbContext(options)
{
    // DbSets for Catalog write models go here
}
