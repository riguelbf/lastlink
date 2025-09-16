using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Platform.SharedKernel.Infrastructure.Persistence.Uow;
using ModularMonolith.Platform.SharedKernel.Messaging;
using ModularMonolith.Platform.SharedKernel.Tracing;
using Modules.Catalog.Presentation;
using WebApp.Modules.Catalog.Application.Consumers;
using WebApp.Modules.Catalog.Infra;
using WebApp.Modules.Catalog.Presentation;

namespace WebApp.Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalog(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Catalog services, DbContexts, Repositories, etc.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CatalogReadController>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRTracingBehavior<,>));

        // DbContexts: separate write and read stores (two databases)
        services.AddDbContext<CatalogWriteDbContext>(opt =>
            opt.UseInMemoryDatabase("catalog-write"));

        services.AddDbContext<CatalogReadDbContext>(opt =>
            opt.UseInMemoryDatabase("catalog-read"));

        // Unit of Work for write database and fluent UoW helper
        services.AddUnitOfWork<CatalogWriteDbContext, CatalogReadDbContext>();

        // Register in-process consumer for Billing.InvoiceCreated events
        services.AddScoped<IEventConsumer, InvoiceCreatedConsumer>();
        return services;
    }

    public static IEndpointRouteBuilder MapCatalog(this IEndpointRouteBuilder endpoints)
    {
        // Register minimal API endpoints here if needed.
        return endpoints;
    }
}
