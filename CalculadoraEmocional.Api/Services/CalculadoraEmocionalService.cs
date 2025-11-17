using CalculadoraEmocional.Api.Models;

namespace CalculadoraEmocional.Api.Services
{
    public class CalculadoraEmocionalService
    {
        public ResultadoEmocionalResponse Calcular(CheckinRequest request)
        {
            // Índice de Bem-Estar = (humor + foco + pausas) / 3
            double indice = (request.Humor + request.Foco + request.Pausas) / 3.0;

            // Risco de burnout = (horas trabalhadas × 0.2) − (pausas × 0.3)
            double risco = (request.HorasTrabalhadas * 0.2) - (request.Pausas * 0.3);

            string nivelRisco;
            if (risco <= 0)
                nivelRisco = "baixo";
            else if (risco < 2)
                nivelRisco = "moderado";
            else
                nivelRisco = "alto";

            bool deveDispararAlerta = nivelRisco == "alto";

            string recomendacao = deveDispararAlerta
                ? "Risco de burnout alto. Recomendamos conversar com o gestor e reduzir a carga de trabalho."
                : "Continue cuidando do seu bem-estar com pausas e uma carga de trabalho equilibrada.";

            return new ResultadoEmocionalResponse
            {
                EmpresaId = request.EmpresaId,
                ColaboradorId = request.ColaboradorId,
                DataReferencia = request.DataReferencia,
                IndiceBemEstar = indice,
                RiscoBurnout = risco,
                NivelRisco = nivelRisco,
                DeveDispararAlerta = deveDispararAlerta,
                Recomendacao = recomendacao
            };
        }
    }
}
