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
    public class ZonasController : ControllerBase
    {
        private readonly IZonaRepository _zonaRepository;

        public ZonasController(IZonaRepository zonaRepository)
        {
            _zonaRepository = zonaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAll()
        {
            var res = await _zonaRepository.GetAll();
            return Ok(res);
        }
    }
}

