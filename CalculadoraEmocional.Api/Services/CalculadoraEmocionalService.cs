using CalculadoraEmocional.Api.Data;
using CalculadoraEmocional.Api.Entities;
using CalculadoraEmocional.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

            var agora = DateTime.UtcNow;

            var checkin = new Checkin
            {
                IdUsuario = request.ColaboradorId,
                DataCheckin = agora,
                Humor = request.Humor,
                Foco = request.Foco,
                MinutosPausas = request.Pausas,
                HorasTrabalhadas = (decimal)request.HorasTrabalhadas,
                Observacoes = request.Observacoes,
                Tags = request.Tags,
                Origem = "API"
            };

            await _context.Checkins.AddAsync(checkin);
            await _context.SaveChangesAsync();

            var resposta = new ResultadoEmocionalResponse
            {
                IdCheckin = checkin.IdCheckin,
                EmpresaId = request.EmpresaId,
                ColaboradorId = request.ColaboradorId,
                DataReferencia = DateOnly.FromDateTime(agora),
                IndiceBemEstar = indice,
                RiscoBurnout = risco,
                NivelRisco = nivelRisco,
                DeveDispararAlerta = deveDispararAlerta,
                Recomendacao = recomendacao,
                Observacoes = request.Observacoes,
                Tags = request.Tags
            };

            resposta.Links = CriarLinksIndice(resposta.EmpresaId, resposta.ColaboradorId);

            return resposta;
        }

        public async Task<(List<ResultadoEmocionalResponse> Items, int TotalCount)> ListarIndicesAsync(
            int? colaboradorId = null,
            int page = 1,
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Checkins.AsQueryable();

            if (colaboradorId.HasValue)
            {
                long idUsuario = colaboradorId.Value;
                query = query.Where(c => c.IdUsuario == idUsuario);
            }

            var totalCount = await query.CountAsync();

            var checkins = await query
                .OrderByDescending(c => c.DataCheckin)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = checkins.Select(c =>
            {
                var pausas = c.MinutosPausas ?? 0;
                var horasTrabalhadas = (double)(c.HorasTrabalhadas ?? 0m);

                double indice = (c.Humor + c.Foco + pausas) / 3.0;
                double risco = (horasTrabalhadas * 0.2) - (pausas * 0.3);

                string nivelRisco;
                if (risco <= 0)
                    nivelRisco = "baixo";
                else if (risco < 2)
                    nivelRisco = "moderado";
                else
                    nivelRisco = "alto";

                bool deveDispararAlerta = nivelRisco == "alto";

                var res = new ResultadoEmocionalResponse
                {
                    IdCheckin = c.IdCheckin,
                    EmpresaId = 0,
                    ColaboradorId = (int)c.IdUsuario,
                    DataReferencia = DateOnly.FromDateTime(c.DataCheckin),
                    IndiceBemEstar = indice,
                    RiscoBurnout = risco,
                    NivelRisco = nivelRisco,
                    DeveDispararAlerta = deveDispararAlerta,
                    Recomendacao = deveDispararAlerta
                        ? "Risco de burnout alto. Acompanhe de perto este colaborador."
                        : "Índice dentro da normalidade.",
                    Observacoes = c.Observacoes,
                    Tags = c.Tags
                };

                res.Links = CriarLinksIndice(res.EmpresaId, res.ColaboradorId);
                return res;
            }).ToList();

            return (items, totalCount);
        }

        public async Task<ResultadoEmocionalResponse?> AtualizarCheckinAsync(
            long idCheckin,
            AtualizarCheckinRequest request)
        {
            var checkin = await _context.Checkins
                .FirstOrDefaultAsync(c => c.IdCheckin == idCheckin);

            if (checkin == null)
                return null;

            checkin.Humor = request.Humor;
            checkin.Foco = request.Foco;
            checkin.MinutosPausas = request.Pausas;
            checkin.HorasTrabalhadas = (decimal)request.HorasTrabalhadas;
            checkin.Observacoes = request.Observacoes;
            checkin.Tags = request.Tags;

            await _context.SaveChangesAsync();

            var pausas = checkin.MinutosPausas ?? 0;
            var horasTrabalhadas = (double)(checkin.HorasTrabalhadas ?? 0m);

            double indice = (checkin.Humor + checkin.Foco + pausas) / 3.0;
            double risco = (horasTrabalhadas * 0.2) - (pausas * 0.3);

            string nivelRisco;
            if (risco <= 0)
                nivelRisco = "baixo";
            else if (risco < 2)
                nivelRisco = "moderado";
            else
                nivelRisco = "alto";

            bool deveDispararAlerta = nivelRisco == "alto";

            var resposta = new ResultadoEmocionalResponse
            {
                IdCheckin = checkin.IdCheckin,
                EmpresaId = 0,
                ColaboradorId = (int)checkin.IdUsuario,
                DataReferencia = DateOnly.FromDateTime(checkin.DataCheckin),
                IndiceBemEstar = indice,
                RiscoBurnout = risco,
                NivelRisco = nivelRisco,
                DeveDispararAlerta = deveDispararAlerta,
                Recomendacao = deveDispararAlerta
                    ? "Risco de burnout alto. Acompanhe de perto este colaborador."
                    : "Índice dentro da normalidade.",
                Observacoes = checkin.Observacoes,
                Tags = checkin.Tags
            };

            resposta.Links = CriarLinksIndice(resposta.EmpresaId, resposta.ColaboradorId);

            return resposta;
        }

        public async Task<bool> ExcluirCheckinAsync(long idCheckin)
        {
            var checkin = await _context.Checkins
                .FirstOrDefaultAsync(c => c.IdCheckin == idCheckin);

            if (checkin == null)
                return false;

            _context.Checkins.Remove(checkin);
            await _context.SaveChangesAsync();
            return true;
        }

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
