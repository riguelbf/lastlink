using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using ModularMonolithTemplate.Billing.Infrastructure.Invoice.Commands;
using ModularMonolithTemplate.Infrastructure.Http.Endpoints;

namespace ModularMonolithTemplate.Billing.Presentation.Endpoints;

public sealed class BillingEndpoints : IEndpointRegistrar
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        // Health/ping for the module
        app.MapGet("/api/billing/ping", () => Results.Ok(new { module = "billing", status = "ok" }))
            .WithName("Billing_Ping")
            .WithTags("Billing")
            .Produces(StatusCodes.Status200OK)
            .WithOpenApi(op =>
            {
                op.Summary = "Ping the Billing module";
                op.Description = "Returns a simple status payload for the Billing module.";
                return op;
            });

        // Invoices (v1). For now we encode the version into the route.
        var invoices = app.MapGroup("/api/v1/billing/invoices").WithTags("Billing.Invoices");

        invoices.MapPost("/", async (CreateInvoiceRequest req, IMediator mediator, HttpContext ctx, CancellationToken ct) =>
        {
            var id = await mediator.Send(new CreateInvoiceCommand(req.CustomerId, req.Amount), ct);
            var dto = new CreateInvoiceResponse(id, req.CustomerId, req.Amount);
            return Results.Created($"/api/v1/billing/invoices/{id}", dto);
        })
        .WithName("Billing_CreateInvoice")
        .Produces<CreateInvoiceResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi(op =>
        {
            op.Summary = "Create a new invoice";
            op.Description = "Creates an invoice and returns its basic data.";
            return op;
        });

        invoices.MapGet("/{id:guid}", (Guid id) => Results.Ok(new { id }))
            .WithName("Billing_GetInvoiceById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(op =>
            {
                op.Summary = "Get invoice by id";
                op.Description = "Returns a placeholder invoice by id (demo).";
                return op;
            });
    }

    public sealed record CreateInvoiceRequest(string CustomerId, decimal Amount);
    public sealed record CreateInvoiceResponse(Guid Id, string CustomerId, decimal Amount);
}
