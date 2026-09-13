using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.DTOs;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemEntity>> ListFoundItemsAsync();
    Task<IEnumerable<ItemEntity>> ListLostItemsAsync();
    Task UploadFoundItemAsync(UploadItemDto item);
    Task UpdateItemStatusAsync(int Id, int StatusId);
    Task RemoveItemAsync(int Id);
}