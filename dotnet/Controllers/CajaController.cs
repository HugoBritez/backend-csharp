using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Models.ViewModels;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/caja")]
    // [Authorize]
    public class CajaController : ControllerBase
    {
        private readonly ICajaRepository _cajaRepository;

        public CajaController(ICajaRepository cajaRepository)
        {
            _cajaRepository = cajaRepository;
        }

        [HttpGet("abierta")]
        public async Task<ActionResult<IEnumerable<CajaViewModel>>> VerificarCajaAbierta([FromQuery] uint operador, [FromQuery] uint moneda)
        {
            var res = await _cajaRepository.VerificarCajaAbierta(operador, moneda);
            return Ok(res);
        } 

    }
}
