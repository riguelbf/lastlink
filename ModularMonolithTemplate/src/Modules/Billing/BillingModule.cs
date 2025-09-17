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
using Infrastructure.Config;

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
        // Prefer connection strings provided by Aspire via AppHost references:
        // ConnectionStrings:BillingWrite and ConnectionStrings:BillingRead
        var writeConn = configuration.GetConnectionString("BillingWrite") ?? EnvVars.Get(
            "ConnectionStrings__BillingWrite",
            "Server=mysql;Port=3306;Database=billing-write;User=app;Password=apppwd;SslMode=None;AllowPublicKeyRetrieval=True")!;
        services.AddDbContext<BillingWriteDbContext>(opt =>
            opt.UseMySql(writeConn, ServerVersion.AutoDetect(writeConn), mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            }));

        var readConn = configuration.GetConnectionString("BillingRead") ?? EnvVars.Get(
            "ConnectionStrings__BillingRead",
            "Server=mysql;Port=3306;Database=billing-read;User=app;Password=apppwd;SslMode=None;AllowPublicKeyRetrieval=True")!;
        services.AddDbContext<BillingReadDbContext>(opt =>
            opt.UseMySql(readConn, ServerVersion.AutoDetect(readConn), mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            }));

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
