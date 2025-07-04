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
    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoRepository _departamentosRepository;

        public DepartamentosController(IDepartamentoRepository departamentoRepository)
        {
            _departamentosRepository = departamentoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetAll([FromQuery] string? busqueda)
        {
            var res = await _departamentosRepository.GetAll(busqueda);
            return Ok(res);
        }
    }
}

