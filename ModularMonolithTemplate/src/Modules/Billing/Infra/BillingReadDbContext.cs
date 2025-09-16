using Microsoft.EntityFrameworkCore;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;

namespace WebApp.Modules.Billing.Infra;

public class BillingReadDbContext(DbContextOptions<BillingReadDbContext> options) : AppDbContext(options)
{
    // DbSets for Billing read models go here
}
