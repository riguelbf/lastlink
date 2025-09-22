using Microsoft.AspNetCore.Mvc;

namespace WebApp.Modules.Catalog.Presentation;

[ApiController]
[Route("api/catalog/[controller]")]
public class CatalogController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { module = "catalog", status = "ok" });
}
