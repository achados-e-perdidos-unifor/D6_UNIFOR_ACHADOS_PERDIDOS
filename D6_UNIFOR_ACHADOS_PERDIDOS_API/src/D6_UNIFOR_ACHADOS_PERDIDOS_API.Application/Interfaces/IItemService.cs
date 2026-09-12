using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemEntity>> GetAllItemsAsync();
}