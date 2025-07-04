using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos.Sucursal;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;
namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class CiudadesController : ControllerBase
    {
        private readonly ICiudadesRepository _ciudadesRepository;

        public CiudadesController(ICiudadesRepository ciudadesRepository)
        {
            _ciudadesRepository = ciudadesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciudad>>> GetAll([FromQuery] string? busqueda)
        {
            var res = await _ciudadesRepository.GetAll(busqueda);
            return Ok(res);
        }
    }
}

