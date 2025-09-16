using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Modules.Catalog.Infra;

namespace Modules.Catalog.Presentation;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/catalog/invoices")]
public class CatalogReadController(CatalogReadDbContext db) : ControllerBase
{
    public sealed record CatalogInvoiceDto(Guid BillingInvoiceId, string CustomerId, decimal Amount, DateTime CreatedAtUtc);

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CatalogInvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await db.Invoices.AsNoTracking().OrderByDescending(i => i.CreatedAtUtc).ToListAsync(ct);
        var dtos = items.Adapt<List<CatalogInvoiceDto>>();
        return Ok(dtos);
    }
}
