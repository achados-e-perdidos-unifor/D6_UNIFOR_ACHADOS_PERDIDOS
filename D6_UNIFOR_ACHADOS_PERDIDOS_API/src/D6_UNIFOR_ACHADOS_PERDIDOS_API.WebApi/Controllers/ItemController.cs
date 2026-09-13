using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.DTOs;
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

    [HttpGet("ListFoundItems")]
    public async Task<IActionResult> ListFoundItemsAsync()
    {
        var items = await _itemService.ListFoundItemsAsync();
        return Ok(items);
    }
    
    [HttpGet("ListLostItems")]
    public async Task<IActionResult> ListLostItemsAsync()
    {
        var items = await _itemService.ListLostItemsAsync();
        return Ok(items);
    }

    [HttpPost("UploadFoundItem")]
    public async Task<IActionResult> UploadFoundItemAsync([FromBody] UploadItemDto item)
    {
        await _itemService.UploadFoundItemAsync(item);
        return Ok();
    }

    [HttpPut("UpdateItemStatus/{id}")]
    public async Task<IActionResult> UpdateItemStatusAsync(int id, [FromBody] int statusId)
    {
        await _itemService.UpdateItemStatusAsync(id, statusId);
        return Ok();
    }

    [HttpDelete("RemoveItem/{id}")]
    public async Task<IActionResult> RemoveItemAsync(int id)
    {
        await _itemService.RemoveItemAsync(id);
        return Ok();
    }
}