using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;
    private readonly IItemService  _itemService;
    
    public ItemController(ILogger<ItemController> logger, IItemService itemService)
    {
        _logger = logger;
        _itemService = itemService;
    }

    [HttpGet("GetAllItems")]
    public async Task<IActionResult> GetAllItemsAsync()
    {
        var items = await _itemService.GetAllItemsAsync();
        return Ok(items);
    }
}