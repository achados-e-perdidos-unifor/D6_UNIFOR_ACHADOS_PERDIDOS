using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public async Task<IEnumerable<ItemEntity>> ListFoundItemsAsync()
    {
        var items = await _itemRepository.ListFoundItemsAsync();
        return items;
    }
    
    public async Task<IEnumerable<ItemEntity>> ListLostItemsAsync()
    {
        var items = await _itemRepository.ListLostItemsAsync();
        return items;
    }
}