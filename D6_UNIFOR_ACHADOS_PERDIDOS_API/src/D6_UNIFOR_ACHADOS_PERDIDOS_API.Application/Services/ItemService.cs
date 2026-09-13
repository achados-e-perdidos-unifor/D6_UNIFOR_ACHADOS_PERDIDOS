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

    public async Task UploadFoundItemAsync(ItemEntity item)
    {
        await _itemRepository.UploadFoundItemAsync(item);
    }

    public async Task UpdateItemStatusAsync(int Id, string Status)
    {
        await _itemRepository.UpdateItemStatusAsync(Id, Status);
    }

    public async Task RemoveItemAsync(int Id)
    {
        await _itemRepository.RemoveItemAsync(Id);
    }
}