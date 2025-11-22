namespace CalculadoraEmocional.Api.Entities
{
    public class IndiceEmocional
    {
        public long IdIndice { get; set; }          
        public long IdUsuario { get; set; }         
        public DateTime DataReferencia { get; set; }
        public int IdVersaoCalculo { get; set; } 
        public double IndiceBemEstar { get; set; } 
        public double RiscoBurnout { get; set; } 
        public string? DetalhesCalculo { get; set; }
    }
}
