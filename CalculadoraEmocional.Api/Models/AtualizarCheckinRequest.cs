namespace CalculadoraEmocional.Api.Models
{
    public class AtualizarCheckinRequest
    {
        public int Humor { get; set; }
        public int Foco { get; set; }
        public int Pausas { get; set; }
        public double HorasTrabalhadas { get; set; }
        public string? Observacoes { get; set; }
        public string? Tags { get; set; }
    }
}
