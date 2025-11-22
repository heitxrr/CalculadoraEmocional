using CalculadoraEmocional.Api.Models;
using CalculadoraEmocional.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraEmocional.Api.Controllers
{
    [ApiController]
    [Route("api/v2/calculadora-emocional")]
    public class CalculadoraEmocionalV2Controller : ControllerBase
    {
        private readonly CalculadoraEmocionalService _service;

        public CalculadoraEmocionalV2Controller(CalculadoraEmocionalService service)
        {
            _service = service;
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<ResultadoEmocionalResponse>> CriarCheckinV2([FromBody] CheckinRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.CalcularERegistrarAsync(request);
            return Ok(resultado);
        }

        [HttpGet("indices")]
        public async Task<ActionResult<object>> ListarIndicesV2(
            [FromQuery] int? colaboradorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _service.ListarIndicesAsync(colaboradorId, page, pageSize);

            var response = new
            {
                totalCount,
                page,
                pageSize,
                items
            };

            return Ok(response);
        }

        [HttpPut("checkin/{id:long}")]
        public async Task<ActionResult<ResultadoEmocionalResponse>> AtualizarCheckinV2(
            long id,
            [FromBody] AtualizarCheckinRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.AtualizarCheckinAsync(id, request);

            if (resultado == null)
                return NotFound(new { mensagem = $"Check-in {id} não encontrado." });

            return Ok(resultado);
        }
    }
}
