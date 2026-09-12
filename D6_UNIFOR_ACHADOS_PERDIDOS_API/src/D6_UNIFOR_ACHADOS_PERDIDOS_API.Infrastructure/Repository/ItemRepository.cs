using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using Dapper;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Data;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Repository;

public class ItemRepository : IItemRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ItemRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ItemEntity>> GetAllItemsAsync()
    {
        throw new NotImplementedException();
    }
}