namespace CalculadoraEmocional.Api.Entities
{
    public class Checkin
    {
        public long IdCheckin { get; set; }
        public long IdUsuario { get; set; }
        public DateTime DataCheckin { get; set; } 
        public int Humor { get; set; }            
        public int Foco { get; set; }             
        public int? MinutosPausas { get; set; }
        public decimal? HorasTrabalhadas { get; set; }
        public string? Observacoes { get; set; }  
        public string? Tags { get; set; }         
        public string? Origem { get; set; }
    }
}
