using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos.Sucursal;
using Microsoft.AspNetCore.Authorization;
namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalRepository _sucursalRepository;

        public SucursalesController(ISucursalRepository sucursalRepository)
        {
            _sucursalRepository = sucursalRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucursalDTO>>> GetSucursales(
            [FromQuery] uint? operador = null,
            [FromQuery] uint? matriz = null
        )
        {
            var sucursales = await _sucursalRepository.GetSucursales(operador, matriz);
            
            return Ok(sucursales);
        }
    }
}

