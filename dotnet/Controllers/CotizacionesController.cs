using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Models.Entities;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/cotizaciones")]
    // [Authorize]
    public class CotizacionesController : ControllerBase
    {
        private readonly ICotizacionRepository _cotizacionRepository;

        public CotizacionesController(ICotizacionRepository cotizacionRepository)
        {
            _cotizacionRepository = cotizacionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cotizacion>>> GetCotizacionesHoy()
        {
            var res = await _cotizacionRepository.GetCotizacionesHoy();
            return Ok(res);
        }

    }
}
