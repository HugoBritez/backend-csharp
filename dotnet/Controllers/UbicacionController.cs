using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/ubicaciones")]
    // [Authorize]
    public class UbicacionController : ControllerBase
    {
        private readonly IUbicacionesRepository _ubicacionesRepository;

        public UbicacionController(IUbicacionesRepository ubicacionesRepository)
        {
            _ubicacionesRepository = ubicacionesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones(
            [FromQuery] string? busqueda
        )
        {
            var ubicaciones = await _ubicacionesRepository.GetUbicaciones(busqueda);
            return Ok(ubicaciones);
        }

    }
}
