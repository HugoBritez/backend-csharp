using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Services.Interfaces;
using Api.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/personal")]
    // [Authorize]
    public class PersonalController : ControllerBase
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IPersonalService _personalService;
        private readonly ILogger<PersonalController> _logger;

        public PersonalController(
            IPersonaRepository personaRepository,
            IPersonalService personalService,
            ILogger<PersonalController> logger)
        {
            _personaRepository = personaRepository;
            _personalService = personalService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CrearPersonal([FromBody] CrearPersonaDTO CrearPersonaDTO)
        {
            _logger.LogInformation("=== LLEGÓ AL CONTROLADOR CrearPersonal ===");

            // Verificar validaciones del modelo
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("=== ERRORES DE VALIDACIÓN ===");
                foreach (var kvp in ModelState)
                {
                    if (kvp.Value.Errors.Any())
                    {
                        _logger.LogWarning($"Campo: {kvp.Key}");
                        foreach (var error in kvp.Value.Errors)
                        {
                            _logger.LogWarning($"  Error: {error.ErrorMessage}");
                        }
                    }
                }

                return BadRequest(ModelState);
            }

            _logger.LogInformation("=== VALIDACIONES PASARON ===");
            var res = await _personalService.CrearPersona(CrearPersonaDTO);
            return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonaViewModel>>> GetAll(
            [FromQuery] string? Busqueda,
            [FromQuery] int Tipo
        )
        {

            var res = await _personalService.GetPersonas(
                Busqueda, Tipo
            );

            return Ok(res);
        }


        [HttpGet("ruc")]
        public async Task<ActionResult<CrearPersonaDTO>> GetPorRuc(
            [FromQuery] uint id,
            [FromQuery] int Tipo
        )
        {
            var res = await _personalService.GetPersonaByRuc(id, Tipo);
            return Ok(res);
        }

        [HttpGet("ultimo-codigo")]
        public async Task<ActionResult<string>> GetUltimoCodigo()
        {
            var res = await _personaRepository.GetUltimoCodigoInterno();
            return Ok(res);
        }
    }
}
