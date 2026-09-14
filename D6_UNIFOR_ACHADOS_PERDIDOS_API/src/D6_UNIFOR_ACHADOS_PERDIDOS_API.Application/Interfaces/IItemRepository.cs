using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;

public interface IItemRepository
{
    Task<IEnumerable<ItemEntity>> ListAllItemsAsync();
    Task<IEnumerable<ItemEntity>> ListFoundItemsAsync();
    Task<IEnumerable<ItemEntity>> ListLostItemsAsync();
    Task<IEnumerable<ItemEntity>> ListReturnedItemsAsync();
    Task<ItemEntity> GetItemByIdAsync(int Id);
    Task UploadFoundItemAsync(ItemEntity item);
    Task UpdateAsync(ItemEntity item);
    Task RemoveItemAsync(int Id);
}