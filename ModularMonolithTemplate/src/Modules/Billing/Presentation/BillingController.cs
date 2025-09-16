using Microsoft.AspNetCore.Mvc;

namespace Modules.Billing.Presentation;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { module = "billing", status = "ok" });
}
