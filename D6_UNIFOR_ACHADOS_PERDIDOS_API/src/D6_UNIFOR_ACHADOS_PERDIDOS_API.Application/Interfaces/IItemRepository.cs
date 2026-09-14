using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;

public interface IItemRepository
{
    Task<IEnumerable<ItemEntity>> ListAllItemsAsync();
    Task<IEnumerable<ItemEntity>> ListFoundItemsAsync();
    Task<IEnumerable<ItemEntity>> ListLostItemsAsync();
    Task<IEnumerable<ItemEntity>> ListReturnedItemsAsync();
    Task UploadFoundItemAsync(ItemEntity item);
    Task UpdateItemStatusAsync(int Id, int StatusId);
    Task RemoveItemAsync(int Id);
}