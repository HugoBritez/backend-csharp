using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/monedas")]
    // [Authorize]
    public class MonedasController : ControllerBase
    {
        private readonly IMonedaRepository _monedaRepository;

        public MonedasController(IMonedaRepository monedaRepository)
        {
            _monedaRepository = monedaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moneda>>> GetAll()
        {
            var monedas = await _monedaRepository.GetAll();
            return Ok(monedas);
        }

    }
}
