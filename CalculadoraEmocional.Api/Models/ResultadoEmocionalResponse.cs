namespace CalculadoraEmocional.Api.Models
{
    public class ResultadoEmocionalResponse
    {
        public long IdCheckin { get; set; }

        public int EmpresaId { get; set; }
        public int ColaboradorId { get; set; }
        public DateOnly DataReferencia { get; set; }

        public double IndiceBemEstar { get; set; }
        public double RiscoBurnout { get; set; }
        public string NivelRisco { get; set; } = string.Empty;

        public bool DeveDispararAlerta { get; set; }
        public string Recomendacao { get; set; } = string.Empty;

        public string? Observacoes { get; set; }
        public string? Tags { get; set; }

        public List<LinkResource> Links { get; set; } = new();
    }
}
