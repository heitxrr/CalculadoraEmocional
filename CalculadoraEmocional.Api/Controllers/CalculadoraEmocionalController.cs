using CalculadoraEmocional.Api.Models;
using CalculadoraEmocional.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraEmocional.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calculadora-emocional")]
    public class CalculadoraEmocionalController : ControllerBase
    {
        private readonly CalculadoraEmocionalService _service;
        private readonly ILogger<CalculadoraEmocionalController> _logger;

        public CalculadoraEmocionalController(
            CalculadoraEmocionalService service,
            ILogger<CalculadoraEmocionalController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("checkin")]
        [Produces("application/json")]
        public async Task<ActionResult<ResultadoEmocionalResponse>> RealizarCheckin([FromBody] CheckinRequest request)
        {
            _logger.LogInformation(
                "Recebido check-in para EmpresaId={EmpresaId}, ColaboradorId={ColaboradorId}, Data={Data}",
                request.EmpresaId,
                request.ColaboradorId,
                request.DataReferencia);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido no check-in para ColaboradorId={ColaboradorId}", request.ColaboradorId);
                return BadRequest(ModelState);
            }

            var resultado = await _service.CalcularERegistrarAsync(request);

            if (resultado.DeveDispararAlerta)
            {
                _logger.LogWarning(
                    "Risco de burnout ALTO detectado. EmpresaId={EmpresaId}, ColaboradorId={ColaboradorId}, Risco={Risco}",
                    resultado.EmpresaId,
                    resultado.ColaboradorId,
                    resultado.RiscoBurnout);
            }
            else
            {
                _logger.LogInformation(
                    "Check-in processado com sucesso. EmpresaId={EmpresaId}, ColaboradorId={ColaboradorId}, NivelRisco={Nivel}",
                    resultado.EmpresaId,
                    resultado.ColaboradorId,
                    resultado.NivelRisco);
            }

            return Ok(resultado);
        }

        // GET: api/v1/calculadora-emocional/indices?colaboradorId=10&page=1&pageSize=10
        [HttpGet("indices")]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResult<ResultadoEmocionalResponse>>> ListarIndices(
            [FromQuery] int? colaboradorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation(
                "Listando índices. ColaboradorId={ColaboradorId}, Page={Page}, PageSize={PageSize}",
                colaboradorId,
                page,
                pageSize);

            var (items, totalCount) = await _service.ListarIndicesAsync(colaboradorId, page, pageSize);

            Response.Headers["X-Total-Count"] = totalCount.ToString();

            var resultadoPaginado = new PagedResult<ResultadoEmocionalResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };

            _logger.LogInformation(
                "Consulta de índices concluída. TotalCount={TotalCount}, ItemsPagina={ItemsPagina}",
                totalCount,
                items.Count);

            return Ok(resultadoPaginado);
        }
    }
}
