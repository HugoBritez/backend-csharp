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
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;

        public AreaController(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAll()
        {
            var res = await _areaRepository.GetAll();
            return Ok(res);
        }
    }
}

