using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Models.Entities;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/metodospago")]
    // [Authorize]
    public class MetodosPagoController : ControllerBase
    {
        private readonly IMetodoPagoRepository _metodosPagoRepository;

        public MetodosPagoController(IMetodoPagoRepository metodoPagoRepository)
        {
            _metodosPagoRepository = metodoPagoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPago>>> GetAll()
        {
            var res = await _metodosPagoRepository.GetAll();
            return Ok(res);
        }
    }
}
