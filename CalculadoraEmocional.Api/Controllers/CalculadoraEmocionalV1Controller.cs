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

        public CalculadoraEmocionalController(CalculadoraEmocionalService service)
        {
            _service = service;
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<ResultadoEmocionalResponse>> RealizarCheckin([FromBody] CheckinRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.CalcularERegistrarAsync(request);
            return Ok(resultado);
        }

        [HttpGet("indices")]
        public async Task<ActionResult<object>> ListarIndices(
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
    }
}
