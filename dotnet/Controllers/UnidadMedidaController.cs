using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Api.Models.Entities;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/unidadmedidas")]
    // [Authorize]
    public class UnidadMedidasController : ControllerBase
    {
        private readonly IUnidadMedidaRepository _unidadMedidaRepository;

        public UnidadMedidasController(IUnidadMedidaRepository unidadMedidaRepository)
        {
            _unidadMedidaRepository = unidadMedidaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadMedida>>> GetProveedores(
            [FromQuery] string? busqueda
        )
        {
            var unidad = await _unidadMedidaRepository.GetUnidadMedidas(busqueda);
            return Ok(unidad);
        }
    }
}
