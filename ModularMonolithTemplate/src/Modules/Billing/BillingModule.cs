using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Uow;
using ModularMonolith.Platform.SharedKernel.Tracing;
using Modules.Billing.Infra;
using WebApp.Modules.Billing.Infra;
using Modules.Billing.Presentation;

namespace Modules.Billing;

public static class BillingModule
{
    public static IServiceCollection AddBilling(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Billing services, DbContexts, Repositories, etc.
        // e.g., services.AddDbContext<Billing.Infrastructure.BillingDbContext>(...);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<InvoicesController>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRTracingBehavior<,>));

        // DbContexts: separate write and read stores (two databases)
        services.AddDbContext<BillingWriteDbContext>(opt =>
            opt.UseInMemoryDatabase("billing-write"));

        services.AddDbContext<BillingReadDbContext>(opt =>
            opt.UseInMemoryDatabase("billing-read"));

        // Unit of Work for write database and fluent UoW helper
        services.AddUnitOfWork<BillingWriteDbContext, BillingReadDbContext>();

        // Map base AppDbContext to the write store for outbox publisher
        services.AddScoped<AppDbContext>(sp => sp.GetRequiredService<BillingWriteDbContext>());
        return services;
    }

    public static IEndpointRouteBuilder MapBilling(this IEndpointRouteBuilder endpoints)
    {
        // Register minimal API endpoints here if needed.
        // endpoints.MapGet("/api/billing/ping", () => Results.Ok("billing ok"));
        return endpoints;
    }
}
