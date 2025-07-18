using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Model.Entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Implementations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/crm")]
    // [Authorize]
    public class CRMController : ControllerBase
    {
        private readonly ICRMService _crmService;
        private readonly IEstadoCRMRepository _estadoCRMRepository;
        private readonly IAgendamientoCRMRepository _agendamientoCRMRepository;
        private readonly IRecordatorioCRMRepository _recordatorioCRMRepository;

        public CRMController(ICRMService crmService, IEstadoCRMRepository estadoCRMRepository, IAgendamientoCRMRepository agendamientoCRMRepository, IRecordatorioCRMRepository recordatorioCRMRepository)
        {
            _crmService = crmService;
            _estadoCRMRepository = estadoCRMRepository;
            _agendamientoCRMRepository = agendamientoCRMRepository;
            _recordatorioCRMRepository = recordatorioCRMRepository;
        }

        // Endpoints para Contactos
        [HttpGet("contactos")]
        public async Task<ActionResult<IEnumerable<ContactoViewModel>>> GetContactos()
        {
            var contactos = await _crmService.GetContactos();
            return Ok(contactos);
        }

        [HttpGet("contactos/{id}")]
        public async Task<ActionResult<ContactoViewModel>> GetContactoById(uint id)
        {
            var contacto = await _crmService.GetContactoById(id);
            if (contacto == null)
                return NotFound();
            return Ok(contacto);
        }

        [HttpPost("contactos")]
        public async Task<ActionResult<ContactoCRM>> CrearContacto([FromBody] ContactoCRM contacto)
        {
            var nuevoContacto = await _crmService.CrearContacto(contacto);
            return CreatedAtAction(nameof(GetContactoById), new { id = nuevoContacto.Codigo }, nuevoContacto);
        }

        [HttpPut("contactos")]
        public async Task<ActionResult<ContactoCRM>> ActualizarContacto([FromBody] ContactoCRM contacto)
        {
            var contactoActualizado = await _crmService.ActualizarContacto(contacto);
            return Ok(contactoActualizado);
        }

        // Endpoints para Oportunidades
        [HttpGet("oportunidades")]
        public async Task<ActionResult<IEnumerable<OportunidadViewModel>>> GetOportunidades(
            [FromQuery(Name = "fechaDesde")] DateTime? fechaInicio,
            [FromQuery(Name = "fechaHasta")] DateTime? fechaFin
        )
        {
            var oportunidades = await _crmService.GetOportunidades(fechaInicio, fechaFin);
            return Ok(oportunidades);
        }

        [HttpGet("oportunidades/{id}")]
        public async Task<ActionResult<OportunidadViewModel>> GetOportunidadById(uint id)
        {
            var oportunidad = await _crmService.GetOportunidadById(id);
            if (oportunidad == null)
                return NotFound();
            return Ok(oportunidad);
        }

        [HttpGet("oportunidades/cliente/{cliente}")]
        public async Task<ActionResult<IEnumerable<OportunidadViewModel>>> GetOportunidadesByCliente(uint cliente)
        {
            var oportunidades = await _crmService.GetOportunidadesByCliente(cliente);
            return Ok(oportunidades);
        }

        [HttpGet("oportunidades/operador/{operador}")]
        public async Task<ActionResult<IEnumerable<OportunidadViewModel>>> GetOportunidadesByOperador(uint operador)
        {
            var oportunidades = await _crmService.GetOportunidadesByOperador(operador);
            return Ok(oportunidades);
        }

        [HttpPost("oportunidades")]
        public async Task<ActionResult<OportunidadCRM>> CrearOportunidad([FromBody] OportunidadCRM oportunidad)
        {
            var nuevaOportunidad = await _crmService.CrearOportunidad(oportunidad);
            return CreatedAtAction(nameof(GetOportunidadById), new { id = nuevaOportunidad.Codigo }, nuevaOportunidad);
        }

        [HttpPut("oportunidades")]
        public async Task<ActionResult<OportunidadCRM>> ActualizarOportunidad([FromBody] OportunidadCRM oportunidad)
        {
            var oportunidadActualizada = await _crmService.ActualizarOportunidad(oportunidad);
            return Ok(oportunidadActualizada);
        }

        // Endpoints para Tareas
        [HttpGet("tareas")]
        public async Task<ActionResult<IEnumerable<TareaCRM>>> GetTareas()
        {
            var tareas = await _crmService.GetTareas();
            return Ok(tareas);
        }

        [HttpGet("tareas/{id}")]
        public async Task<ActionResult<TareaCRM>> GetTareaById(uint id)
        {
            var tarea = await _crmService.GetTareaById(id);
            if (tarea == null)
                return NotFound();
            return Ok(tarea);
        }

        [HttpGet("tareas/oportunidad/{oportunidad}")]
        public async Task<ActionResult<IEnumerable<TareaCRM>>> GetTareasByOportunidad(uint oportunidad)
        {
            var tareas = await _crmService.GetTareasByOportunidad(oportunidad);
            return Ok(tareas);
        }

        [HttpGet("tareas/operador/{operador}")]
        public async Task<ActionResult<IEnumerable<TareaCRM>>> GetTareasByOperador(uint operador)
        {
            var tareas = await _crmService.GetTareasByOperador(operador);
            return Ok(tareas);
        }

        [HttpGet("tareas/contacto/{contacto}")]
        public async Task<ActionResult<IEnumerable<TareaCRM>>> GetTareasByContacto(uint contacto)
        {
            var tareas = await _crmService.GetTareasByContacto(contacto);
            return Ok(tareas);
        }

        [HttpPost("tareas")]
        public async Task<ActionResult<TareaCRM>> CrearTarea([FromBody] TareaCRM tarea)
        {
            var nuevaTarea = await _crmService.CrearTarea(tarea);
            return CreatedAtAction(nameof(GetTareaById), new { id = nuevaTarea.Codigo }, nuevaTarea);
        }

        [HttpPut("tareas")]
        public async Task<ActionResult<TareaCRM>> ActualizarTarea([FromBody] TareaCRM tarea)
        {
            var tareaActualizada = await _crmService.ActualizarTarea(tarea);
            return Ok(tareaActualizada);
        }

        [HttpGet("tipos-tareas")]
        public async Task<ActionResult<IEnumerable<TipoTareaCRM>>> GetTiposTareas()
        {
            var tiposTareas = await _crmService.GetTiposTareas();
            return Ok(tiposTareas);
        }

        [HttpGet("estados")]
        public async Task<ActionResult<IEnumerable<EstadoCRM>>> GetEstados()
        {
            var estados = await _estadoCRMRepository.GetEstados();
            return Ok(estados);
        }

        [HttpPut("estados/{codigo}")]
        public async Task<ActionResult<EstadoCRM>> UpdateDescripcion(uint codigo, [FromBody] string descripcion)
        {
            var estado = await _estadoCRMRepository.UpdateDescripcion(codigo, descripcion);
            return Ok(estado);
        }

        // Endpoints para Agendamientos
        [HttpGet("agendamientos")]
        public async Task<ActionResult<IEnumerable<AgendamientoCRM>>> GetAgendamientos()
        {
            var agendamientos = await _agendamientoCRMRepository.GetAll();
            return Ok(agendamientos);
        }

        [HttpGet("agendamientos/operador/{operador}")]
        public async Task<ActionResult<IEnumerable<AgendamientoCRM>>> GetAgendamientosByOperador(uint operador)
        {
            var agendamientos = await _agendamientoCRMRepository.GetByOperador(operador);
            return Ok(agendamientos);
        }

        [HttpGet("agendamientos/doctor/{doctor}")]
        public async Task<ActionResult<IEnumerable<AgendamientoCRM>>> GetAgendamientosByDoctor(uint doctor)
        {
            var agendamientos = await _agendamientoCRMRepository.GetByDoctor(doctor);
            return Ok(agendamientos);
        }

        [HttpPost("agendamientos")]
        public async Task<ActionResult<AgendamientoCRM>> CreateAgendamiento([FromBody] AgendamientoCRM agendamiento)
        {
            var nuevoAgendamiento = await _agendamientoCRMRepository.Create(agendamiento);
            return CreatedAtAction(nameof(GetAgendamientoById), new { id = nuevoAgendamiento.Codigo }, nuevoAgendamiento);
        }

        [HttpPut("agendamientos")]
        public async Task<ActionResult<AgendamientoCRM>> UpdateAgendamiento([FromBody] AgendamientoCRM agendamiento)
        {
            var agendamientoActualizado = await _agendamientoCRMRepository.Update(agendamiento);
            return Ok(agendamientoActualizado);
        }

        [HttpGet("agendamientos/{id}")]
        public async Task<ActionResult<AgendamientoCRM>> GetAgendamientoById(uint id)
        {
            var agendamiento = await _agendamientoCRMRepository.GetById(id);
            return Ok(agendamiento);
        }


        // Endpoints para RECORDATORIOS

        [HttpGet("recordatorios")]
        public async Task<ActionResult<IEnumerable<RecordatorioCRMViewModel>>> GetRecordatorios()
        {
            var recordatorios = await _recordatorioCRMRepository.GetAll();
            return Ok(recordatorios);
        }

        [HttpGet("recordatorios/{id}")]
        public async Task<ActionResult<RecordatorioCRMViewModel>> GetRecordatorioById(uint id)
        {
            var recordatorio = await _recordatorioCRMRepository.GetById(id);
            if (recordatorio == null)
                return NotFound();
            return Ok(recordatorio);
        }

        [HttpPost("recordatorios")]
        public async Task<ActionResult<RecordatorioCRM>> CreateRecordatorio([FromBody] RecordatorioCRM recordatorio)
        {
            var nuevoRecordatorio = await _recordatorioCRMRepository.Create(recordatorio);
            return CreatedAtAction(nameof(GetRecordatorioById), new { id = nuevoRecordatorio.Codigo }, nuevoRecordatorio);
        }

        [HttpPut("recordatorios")]
        public async Task<ActionResult<RecordatorioCRM>> UpdateRecordatorio([FromBody] RecordatorioCRM recordatorio)
        {
            var recordatorioActualizado = await _recordatorioCRMRepository.Update(recordatorio);
            return Ok(recordatorioActualizado);
        }
    }
}
