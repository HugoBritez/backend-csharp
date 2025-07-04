using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Entities;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/configuraciones")]
    // [Authorize]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly IConfiguracionRepository _configuracionesRepository;

        public ConfiguracionesController(IConfiguracionRepository configuracionRepository)
        {
            _configuracionesRepository = configuracionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Configuracion>> GetById( 
            [FromQuery] uint id
        )
        {
            var conf = await _configuracionesRepository.GetById(id);
            return Ok(conf);
        }
    }
}
