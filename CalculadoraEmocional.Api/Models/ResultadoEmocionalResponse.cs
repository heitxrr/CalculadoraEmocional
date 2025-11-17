namespace CalculadoraEmocional.Api.Models
{
    public class ResultadoEmocionalResponse
    {
        public Guid EmpresaId { get; set; }
        public Guid ColaboradorId { get; set; }
        public DateOnly DataReferencia { get; set; }

        public double IndiceBemEstar { get; set; }
        public double RiscoBurnout { get; set; }
        public string NivelRisco { get; set; } = string.Empty;

        public bool DeveDispararAlerta { get; set; }
        public string Recomendacao { get; set; } = string.Empty;
    }
}
