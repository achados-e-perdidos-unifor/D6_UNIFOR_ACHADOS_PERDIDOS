using Microsoft.AspNetCore.Mvc;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;
    
    public ItemController(ILogger<ItemController> logger)
    {
        _logger = logger;
    }
}