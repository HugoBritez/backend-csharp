using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos.Deposito;
using Microsoft.AspNetCore.Authorization;
namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class DepositosController : ControllerBase
    {
        private readonly IDepositosRepository _depositosRepository;

        public DepositosController(IDepositosRepository depositosRepository)
        {
            _depositosRepository = depositosRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositoDTO>>> GetDepositos(
            [FromQuery] uint? sucursal = null,
            [FromQuery] uint? usuario = null,
            [FromQuery] string? descripcion = null
        )
        {
            var depositos = await _depositosRepository.GetDepositos(sucursal, usuario, descripcion);
            
            return Ok(depositos);
        }
    }
}

