namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.DTOs
{
    public record UploadItemDto
    {
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public string? Categoria { get; set; }

        public string? LocalEncontro { get; set; }

        public DateTime DataEncontro { get; set; }

        public string Status { get; set; } = "ENCONTRADO";

        public string? NomeResponsavel { get; set; }

        public string? ContatoResponsavel { get; set; }
    };
}
