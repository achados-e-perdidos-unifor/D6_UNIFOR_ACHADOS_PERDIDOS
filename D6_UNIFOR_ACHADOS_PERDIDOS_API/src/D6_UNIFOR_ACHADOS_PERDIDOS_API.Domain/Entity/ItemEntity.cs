using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Enums;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;

public class ItemEntity
{
    public long? Id { get; private set; }
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public string? Categoria { get; private set; }
    public string? LocalEncontro { get; private set; }
    public DateTime DataEncontro { get; private set; }
    public int StatusId { get; private set; }
    public string? NomeResponsavel { get; private set; }
    public string? ContatoResponsavel { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected ItemEntity() { }

    public ItemEntity(string nome, string descricao, string categoria, string localEncontro, DateTime dataEncontro, int statusId, string? nomeResponsavel, string? contatoResponsavel)
    {
        ValidateStatusId(statusId);
        ValidateFoundDate(dataEncontro);

        Nome = nome;
        Descricao = descricao;
        Categoria = categoria;
        LocalEncontro = localEncontro;
        DataEncontro = dataEncontro;
        StatusId = statusId;
        NomeResponsavel = nomeResponsavel;
        ContatoResponsavel = contatoResponsavel;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(int statusId)
    {
        ValidateStatusId(statusId);

        if (statusId == (int)ItemStatus.ENCONTRADO && StatusId == (int)ItemStatus.DEVOLVIDO)
        {
            throw new ArgumentException("Não é possível atualizar o status de um item devolvido para encontrado.");
        }

        if (statusId == (int)ItemStatus.PERDIDO && StatusId == (int)ItemStatus.DEVOLVIDO)
        {
            throw new ArgumentException("Não é possível atualizar o status de um item devolvido para perdido.");
        }

        StatusId = statusId;
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateStatusId(int statusId)
    {
        if (!Enum.IsDefined(typeof(ItemStatus), statusId))
        {
            throw new ArgumentException("Status inválido.");
        }
    }

    private void ValidateFoundDate(DateTime dataEncontro)
    {
        if (dataEncontro > DateTime.UtcNow)
        {
            throw new ArgumentException("Data de encontro não pode ser após a data atual.");
        }
    }
}