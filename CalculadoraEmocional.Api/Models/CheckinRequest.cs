namespace CalculadoraEmocional.Api.Models
{
    public class CheckinRequest
    {
        public int EmpresaId { get; set; }
        public int ColaboradorId { get; set; }

        public DateOnly DataReferencia { get; set; }

        public int Humor { get; set; }           
        public int Foco { get; set; }            
        public int Pausas { get; set; }          
        public double HorasTrabalhadas { get; set; }
    }
}
