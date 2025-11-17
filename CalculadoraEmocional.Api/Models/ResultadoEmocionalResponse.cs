namespace CalculadoraEmocional.Api.Models
{
    public class ResultadoEmocionalResponse
    {
        public int EmpresaId { get; set; }
        public int ColaboradorId { get; set; }
        public DateOnly DataReferencia { get; set; }

        public double IndiceBemEstar { get; set; }
        public double RiscoBurnout { get; set; }
        public string NivelRisco { get; set; } = string.Empty;

        public bool DeveDispararAlerta { get; set; }
        public string Recomendacao { get; set; } = string.Empty;

        // HATEOAS
        public List<LinkResource> Links { get; set; } = new();
    }
}
