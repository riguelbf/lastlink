using Microsoft.AspNetCore.Mvc;
using SharedKernel.Primitives;

namespace Presentation.Common;

/// <summary>
/// Base API controller that provides common functionality for all API controllers.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    
}
