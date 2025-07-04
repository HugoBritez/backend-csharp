using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/sububicaciones")]
    // [Authorize]
    public class SubUbicacionController : ControllerBase
    {
        private readonly ISububicacionRepository _sububicacionesRepository;

        public SubUbicacionController(ISububicacionRepository sububicacionRepository)
        {
            _sububicacionesRepository = sububicacionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sububicacion>>> GetSubUbicaciones(
            [FromQuery] string? busqueda
        )
        {
            var ubicaciones = await _sububicacionesRepository.GetSubUbicaciones(busqueda);
            return Ok(ubicaciones);
        }

    }
}
