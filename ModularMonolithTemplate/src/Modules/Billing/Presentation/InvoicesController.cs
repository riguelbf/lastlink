using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Billing.Application;

namespace Modules.Billing.Presentation;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/billing/invoices")]
public class InvoicesController(IMediator mediator) : ControllerBase
{
    public sealed record CreateInvoiceRequest(string CustomerId, decimal Amount);
    public sealed record CreateInvoiceResponse(Guid Id, string CustomerId, decimal Amount);

    [HttpPost]
    [ProducesResponseType(typeof(CreateInvoiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new CreateInvoiceCommand(req.CustomerId, req.Amount), ct);
        var dto = new CreateInvoiceResponse(id, req.CustomerId, req.Amount);
        return CreatedAtAction(nameof(GetById), new { id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, dto);
    }

    // For demo purposes only (in-memory DB)
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid id)
        => Ok(new { id });
}
