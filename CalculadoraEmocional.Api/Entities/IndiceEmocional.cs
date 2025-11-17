namespace CalculadoraEmocional.Api.Entities
{
    public class IndiceEmocional
    {
        public int Id { get; set; }              // chave primária

        public int EmpresaId { get; set; }      
        public int ColaboradorId { get; set; }   

        public DateTime DataReferencia { get; set; }

        public double IndiceBemEstar { get; set; }
        public double RiscoBurnout { get; set; }
        public string NivelRisco { get; set; } = string.Empty;

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
