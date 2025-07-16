using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Model.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/crm")]
    // [Authorize]
    public class CRMController : ControllerBase
    {
        private readonly ICRMService _crmService;

        public CRMController(ICRMService crmService)
        {
            _crmService = crmService;
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
        public async Task<ActionResult<IEnumerable<OportunidadViewModel>>> GetOportunidades()
        {
            var oportunidades = await _crmService.GetOportunidades();
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
    }
}
