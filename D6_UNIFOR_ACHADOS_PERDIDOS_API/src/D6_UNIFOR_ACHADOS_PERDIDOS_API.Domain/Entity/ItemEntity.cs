namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

public class ItemEntity
{
    public long? Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public string? Categoria { get; set; }

    public string? LocalEncontro { get; set; }

    public DateTime DataEncontro { get; set; }

    public string Status { get; set; } = "ENCONTRADO";

    public string? NomeResponsavel { get; set; }

    public string? ContatoResponsavel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}