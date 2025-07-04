using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Api.Models.ViewModels;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/")]
    // [Authorize]
    public class FinancieroController : ControllerBase
    {
        private readonly IFinancieroRepository _financieroRepository;

        public FinancieroController(IFinancieroRepository financieroRepository)
        {
            _financieroRepository = financieroRepository;
        }

        [HttpGet("facturacion")]
        public async Task<ActionResult<IEnumerable<TimbradoResult>>> ObtenerDatosFacturacion([FromQuery] uint usuario, [FromQuery] uint sucursal)
        {
            var res = await _financieroRepository.ObtenerDatosFacturacion(usuario, sucursal);
            return Ok(res);
        }
    }
}
