using Microsoft.EntityFrameworkCore;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;

namespace ModularMonolithTemplate.Billing.Infrastructure.Persistence;

public class BillingReadDbContext(DbContextOptions<BillingReadDbContext> options) : AppDbContext(options)
{
    // DbSets for Billing read models go here
}
