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
        [Produces("application/json")]
        public async Task<ActionResult<ResultadoEmocionalResponse>> RealizarCheckin([FromBody] CheckinRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.CalcularERegistrarAsync(request);

            return Ok(resultado);
        }

        [HttpGet("indices")]
        [Produces("application/json")]
        public async Task<ActionResult<List<ResultadoEmocionalResponse>>> ListarIndices([FromQuery] int? colaboradorId)
        {
            var resultados = await _service.ListarIndicesAsync(colaboradorId);
            return Ok(resultados);
        }

    }
}
