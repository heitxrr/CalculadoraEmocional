using System.Threading.Tasks;
using CalculadoraEmocional.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraEmocional.Api.Controllers
{
    [ApiController]
    [Route("api/v3/calculadora-emocional")]
    public class CalculadoraEmocionalV3Controller : ControllerBase
    {
        private readonly CalculadoraEmocionalService _service;

        public CalculadoraEmocionalV3Controller(CalculadoraEmocionalService service)
        {
            _service = service;
        }

        [HttpDelete("checkin/{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirCheckin(long id)
        {
            var removido = await _service.ExcluirCheckinAsync(id);

            if (!removido)
                return NotFound(new { mensagem = $"Check-in {id} não encontrado." });

            return NoContent();
        }
    }
}
