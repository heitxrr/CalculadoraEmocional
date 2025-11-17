namespace CalculadoraEmocional.Api.Models
{
    public class CheckinRequest
    {
        public Guid EmpresaId { get; set; }
        public Guid ColaboradorId { get; set; }

        public DateOnly DataReferencia { get; set; }

        public int Humor { get; set; }           // ex: 1 a 5
        public int Foco { get; set; }            // ex: 1 a 5
        public int Pausas { get; set; }          // quantidade de pausas
        public double HorasTrabalhadas { get; set; } // ex: 8.5
    }
}
