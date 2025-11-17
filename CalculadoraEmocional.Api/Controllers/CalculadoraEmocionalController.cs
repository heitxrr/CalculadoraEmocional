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
        public ActionResult<ResultadoEmocionalResponse> RealizarCheckin([FromBody] CheckinRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = _service.Calcular(request);
            return Ok(resultado);
        }
    }
}
