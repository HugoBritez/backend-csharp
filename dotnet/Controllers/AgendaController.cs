using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Services.Interfaces;
using Api.Models.Entities;
using Api.Models.ViewModels;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/agendas")]
    // [Authorize]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendasService _agendaService;
        private readonly IAgendasRepository _agendaRepository;
        private readonly IAgendasNotasRepository _agendasNotasRepository;
        private readonly IAgendaSubvisitaRepository _AgendaSubvisitaRepository;
        private readonly ILocalizacionesRepository _localizacionesRepository;

        public AgendaController(
            IAgendasService agendasService,
            IAgendasRepository agendasRepository,
            IAgendasNotasRepository agendasNotasRepository,
            IAgendaSubvisitaRepository agendaSubvisitaRepository,
            ILocalizacionesRepository localizacionesRepository
        )
        {
            _agendaService = agendasService;
            _agendaRepository = agendasRepository;
            _agendasNotasRepository = agendasNotasRepository;
            _AgendaSubvisitaRepository = agendaSubvisitaRepository;
            _localizacionesRepository = localizacionesRepository;
        }

        [HttpPost("registrar-llegada")]
        public async Task<ActionResult<Agenda>> RegistrarLlegada([FromBody] RegistrarLlegadaDTO data)
        {
            var res = await _agendaService.RegistrarLlegada(data);
            return Ok(res);
        }

        public class RegistrarSalidaDTO
        {
            public uint IdAgenda { get; set; }
        }

        [HttpPost("registrar-salida")]
        public async Task<ActionResult<Agenda>> RegistrarSalida([FromBody] RegistrarSalidaDTO data)
        {
            var res = await _agendaService.RegistrarSalida(data.IdAgenda);
            return Ok(res);
        }

        [HttpPost("reagendar")]
        public async Task<ActionResult<Agenda>> ReagendarVisita([FromBody] ReagendarVisitaDTO data)
        {
            var res = await _agendaService.ReagendarVisita(data);
            return Ok(res);
        }

        [HttpPost("anular")]
        public async Task<ActionResult<Agenda>> AnularVisita([FromBody] uint idAgenda)
        {
            var res = await _agendaService.AnularVisita(idAgenda);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<Agenda>> Crear([FromBody] Agenda agenda)
        {
            var res = await _agendaRepository.Crear(agenda);
            return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendaViewModel>>> GetTodas(
            [FromQuery] string? fechaDesde = null,
            [FromQuery] string? fechaHasta = null,
            [FromQuery] string? cliente = null,
            [FromQuery] string? vendedor = null,
            [FromQuery] int visitado = -1,
            [FromQuery] int estado = -1,
            [FromQuery] int planificacion = -1,
            [FromQuery] int notas = -1,
            [FromQuery] string orden = "0"
        )
        {
            var res = await _agendaRepository.GetTodas(fechaDesde, fechaHasta, cliente, vendedor, visitado, estado, planificacion, notas, orden);
            return Ok(res);
        }

        [HttpGet("notas")]
        public async Task<ActionResult<AgendasNotas?>> GetByAgenda([FromQuery] uint idAgenda)
        {
            var res = await _agendasNotasRepository.GetByAgenda(idAgenda);
            return Ok(res);
        }

        [HttpPost("notas")]
        public async Task<ActionResult<AgendasNotas>> Crear([FromBody] AgendasNotas nota)
        {
            var res = await _agendasNotasRepository.Crear(nota);
            return Ok(res);
        }

        [HttpPost("subvisitas")]
        public async Task<ActionResult<AgendaSubvisita>> Crear([FromBody] AgendaSubvisita subvisita)
        {
            var res = await _AgendaSubvisitaRepository.Crear(subvisita);
            return Ok(res);
        }

        [HttpGet("subvisitas")]
        public async Task<ActionResult<IEnumerable<AgendaSubvisita?>>> GetSubvisitaByAgenda([FromQuery] uint agendaId)
        {
            var res = await _AgendaSubvisitaRepository.GetByAgenda(agendaId);
            return Ok(res);
        }

        [HttpGet("localizacion")]
        public async Task<ActionResult<Localizacion>> GetLocalizacionByAgenda([FromQuery] uint idAgenda)
        {
            var res = await _localizacionesRepository.GetByAgenda(idAgenda);
            return Ok(res);
        }



    }
}
