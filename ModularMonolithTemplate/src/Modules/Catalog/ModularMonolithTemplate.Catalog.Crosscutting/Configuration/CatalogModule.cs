using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolithTemplate.Catalog.Application.Consumers;
using ModularMonolithTemplate.Catalog.Infrastructure.Persistence;
using ModularMonolithTemplate.Catalog.Presentation.Endpoints;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.Uow;
using ModularMonolithTemplate.SharedKernel.Messaging;
using ModularMonolithTemplate.SharedKernel.Tracing;

namespace ModularMonolithTemplate.Catalog.Crosscutting.Configuration;

public static class CatalogModule
{
    public static IServiceCollection AddCatalog(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Catalog services, DbContexts, Repositories, etc.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CatalogEndpoints>());
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
