using CalculadoraEmocional.Api.Data;
using CalculadoraEmocional.Api.Entities;
using CalculadoraEmocional.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraEmocional.Api.Services
{
    public class CalculadoraEmocionalService
    {
        private readonly CalculadoraEmocionalContext _context;

        public CalculadoraEmocionalService(CalculadoraEmocionalContext context)
        {
            _context = context;
        }

        public async Task<ResultadoEmocionalResponse> CalcularERegistrarAsync(CheckinRequest request)
        {
            // Cálculos
            double indice = (request.Humor + request.Foco + request.Pausas) / 3.0;
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

            // Monta entidade de Checkin
            var checkin = new Checkin
            {
                EmpresaId = request.EmpresaId,
                ColaboradorId = request.ColaboradorId,
                DataCheckin = request.DataReferencia.ToDateTime(TimeOnly.MinValue),
                Humor = request.Humor,
                Foco = request.Foco,
                Pausas = request.Pausas,
                HorasTrabalhadas = request.HorasTrabalhadas,
                Observacoes = null
            };

            await _context.Checkins.AddAsync(checkin);

            // Monta entidade de Índice
            var indiceEmocional = new IndiceEmocional
            {
                EmpresaId = request.EmpresaId,
                ColaboradorId = request.ColaboradorId,
                DataReferencia = request.DataReferencia.ToDateTime(TimeOnly.MinValue),
                IndiceBemEstar = indice,
                RiscoBurnout = risco,
                NivelRisco = nivelRisco,
                CriadoEm = DateTime.UtcNow
            };

            await _context.IndicesEmocionais.AddAsync(indiceEmocional);

            // Salva tudo no banco
            await _context.SaveChangesAsync();

            // Monta resposta pra API
            var resposta = new ResultadoEmocionalResponse
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

            resposta.Links = CriarLinksIndice(resposta.EmpresaId, resposta.ColaboradorId);

            return resposta;
        }

        // Listagem de índices com paginação
        public async Task<(List<ResultadoEmocionalResponse> Items, int TotalCount)> ListarIndicesAsync(
            int? colaboradorId = null,
            int page = 1,
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.IndicesEmocionais.AsQueryable();

            if (colaboradorId.HasValue)
            {
                query = query.Where(i => i.ColaboradorId == colaboradorId.Value);
            }

            var totalCount = await query.CountAsync();

            var indices = await query
                .OrderByDescending(i => i.DataReferencia)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = indices.Select(i =>
            {
                var res = new ResultadoEmocionalResponse
                {
                    EmpresaId = i.EmpresaId,
                    ColaboradorId = i.ColaboradorId,
                    DataReferencia = DateOnly.FromDateTime(i.DataReferencia),
                    IndiceBemEstar = i.IndiceBemEstar,
                    RiscoBurnout = i.RiscoBurnout,
                    NivelRisco = i.NivelRisco,
                    DeveDispararAlerta = i.NivelRisco == "alto",
                    Recomendacao = i.NivelRisco == "alto"
                        ? "Risco de burnout alto. Acompanhe de perto este colaborador."
                        : "Índice dentro da normalidade."
                };

                res.Links = CriarLinksIndice(res.EmpresaId, res.ColaboradorId);
                return res;
            }).ToList();

            return (items, totalCount);
        }

        // 🔗 Helper para montar os links HATEOAS
        private List<LinkResource> CriarLinksIndice(int empresaId, int colaboradorId)
        {
            return new List<LinkResource>
            {
                new LinkResource
                {
                    Rel = "self",
                    Href = $"/api/v1/calculadora-emocional/indices?colaboradorId={colaboradorId}",
                    Method = "GET"
                },
                new LinkResource
                {
                    Rel = "post-checkin",
                    Href = "/api/v1/calculadora-emocional/checkin",
                    Method = "POST"
                },
                new LinkResource
                {
                    Rel = "listar-indices",
                    Href = "/api/v1/calculadora-emocional/indices",
                    Method = "GET"
                }
            };
        }
    }
}
