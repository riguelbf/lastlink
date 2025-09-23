using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ModularMonolithTemplate.Catalog.Infrastructure.Persistence;
using ModularMonolithTemplate.Infrastructure.Http.Endpoints;

namespace ModularMonolithTemplate.Catalog.Presentation.Endpoints;

public sealed class CatalogEndpoints : IEndpointRegister
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        // Health/ping for the module
        app.MapGet("/api/catalog/ping", () => Results.Ok(new { module = "catalog", status = "ok" }))
            .WithName("Catalog_Ping")
            .WithTags("Catalog")
            .Produces(StatusCodes.Status200OK)
            .WithOpenApi(op =>
            {
                op.Summary = "Ping the Catalog module";
                op.Description = "Returns a simple status payload for the Catalog module.";
                return op;
            });

        // Read endpoints (v1)
        var invoices = app.MapGroup("/api/v1/catalog/invoices").WithTags("Catalog.Invoices");

        invoices.MapGet("/", async (CatalogReadDbContext db, CancellationToken ct) =>
        {
            var items = await db.Invoices
                .AsNoTracking()
                .OrderByDescending(i => i.CreatedAtUtc)
                .ToListAsync(ct);

            var dtos = items.Adapt<List<CatalogInvoiceDto>>();
            return Results.Ok(dtos);
        })
        .WithName("Catalog_GetInvoices")
        .Produces<IEnumerable<CatalogInvoiceDto>>(StatusCodes.Status200OK)
        .WithOpenApi(op =>
        {
            op.Summary = "List invoices (read model)";
            op.Description = "Returns all invoices as seen by the Catalog read model.";
            return op;
        });
    }

    public sealed record CatalogInvoiceDto(Guid BillingInvoiceId, string CustomerId, decimal Amount, DateTime CreatedAtUtc);
}
