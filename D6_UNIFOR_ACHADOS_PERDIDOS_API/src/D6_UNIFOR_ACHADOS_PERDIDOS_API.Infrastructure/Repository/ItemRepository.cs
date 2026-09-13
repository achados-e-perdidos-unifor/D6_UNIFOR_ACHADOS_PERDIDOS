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

    public async Task<IEnumerable<ItemEntity>> ListFoundItemsAsync()
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
                           WHERE status = 'ENCONTRADO'
                           ORDER BY id;
                           """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<ItemEntity>(sql);
    }
    
    public async Task<IEnumerable<ItemEntity>> ListLostItemsAsync()
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
                           WHERE status = 'PERDIDO'
                           ORDER BY id;
                           """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<ItemEntity>(sql);
    }

    public async Task UploadFoundItemAsync(ItemEntity item)
    {
        var sql = """
            INSERT INTO tb_item (name, description, category, found_location, found_date, status, person_who_found, contact_who_found)
            VALUES (@Name, @Description, @Category, @FoundLocation, @FoundDate, @Status, @PersonWhoFound, @ContactWhoFound)
            """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new
        {
            Name = item.Nome,
            Description = item.Descricao,
            Category = item.Categoria,
            FoundLocation = item.LocalEncontro,
            FoundDate = item.DataEncontro,
            Status = item.Status,
            PersonWhoFound = item.NomeResponsavel,
            ContactWhoFound = item.ContatoResponsavel
        });
    }

    public async Task UpdateItemStatusAsync(int Id, string Status)
    {
        var sql = "UPDATE tb_item SET status = @Status WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new { Id = Id, Status = Status });
    }

    public async Task RemoveItemAsync(int Id)
    {
        var sql = "DELETE FROM tb_item WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new { Id = Id });
    }
}