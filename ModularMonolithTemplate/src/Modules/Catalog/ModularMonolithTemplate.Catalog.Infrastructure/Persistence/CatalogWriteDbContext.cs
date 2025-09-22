using Microsoft.EntityFrameworkCore;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;

namespace ModularMonolithTemplate.Catalog.Infrastructure.Persistence;

public class CatalogWriteDbContext(DbContextOptions<CatalogWriteDbContext> options) : AppDbContext(options)
{
    // DbSets for Catalog write models go here
}
