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
        const string sql = """
                           SELECT
                               id AS Id,
                               name AS Nome,
                               description AS Descricao,
                               category AS Categoria,
                               found_location AS LocalEncontro,
                               found_date AS DataEncontro,
                               status AS Status,
                               person_who_found AS NomeResponsavel,
                               contact_who_found AS ContatoResponsavel,
                               created_at AS CreatedAt,
                               updated_at AS UpdatedAt
                           FROM tb_item
                           ORDER BY id;
                           """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<ItemEntity>(sql);
    }
}