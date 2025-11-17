namespace CalculadoraEmocional.Api.Entities
{
    public class Checkin
    {
        public int Id { get; set; }              // chave primária

        public int EmpresaId { get; set; }       // agora int
        public int ColaboradorId { get; set; }   // agora int

        public DateTime DataCheckin { get; set; }

        public int Humor { get; set; }           // 1 a 5
        public int Foco { get; set; }            // 1 a 5
        public int Pausas { get; set; }          // quantidade de pausas
        public double HorasTrabalhadas { get; set; }

        public string? Observacoes { get; set; }
    }
}
