using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Model.Entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Implementations;
using Storage;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Api.Models.Dtos.CRM;
using Api.Models.Dtos;

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
        private readonly IFileStorageService _fileStorageService;
        private readonly IProyectosColaboradoresRepositoryCRM _proyectosColaboradoresRepository;

        private readonly IEmailService _emailService;

        public CRMController(ICRMService crmService, IEstadoCRMRepository estadoCRMRepository, IAgendamientoCRMRepository agendamientoCRMRepository, IRecordatorioCRMRepository recordatorioCRMRepository, IFileStorageService fileStorageService, IProyectosColaboradoresRepositoryCRM proyectosColaboradoresRepository, IEmailService emailService    )
        {
            _crmService = crmService;
            _estadoCRMRepository = estadoCRMRepository;
            _agendamientoCRMRepository = agendamientoCRMRepository;
            _recordatorioCRMRepository = recordatorioCRMRepository;
            _fileStorageService = fileStorageService;
            _proyectosColaboradoresRepository = proyectosColaboradoresRepository;
            _emailService = emailService;
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
        public async Task<ActionResult<OportunidadCRM>> CrearOportunidad([FromBody] CrearOportunidadDTO oportunidad)
        {
            var nuevaOportunidad = await _crmService.CrearOportunidad(oportunidad);
            return CreatedAtAction(nameof(GetOportunidadById), new { id = nuevaOportunidad.Codigo }, nuevaOportunidad);
        }

        [HttpPut("oportunidades")]
        public async Task<ActionResult<OportunidadCRM>> ActualizarOportunidad([FromBody] CrearOportunidadDTO oportunidad)
        {
            var oportunidadActualizada = await _crmService.ActualizarOportunidad(oportunidad);
            return Ok(oportunidadActualizada);
        }

        [HttpGet("oportunidades/archivadas")]
        public async Task<ActionResult<IEnumerable<OportunidadViewModel>>> GetOportunidadesArchivadas(
            [FromQuery(Name = "fechaDesde")] DateTime? fechaInicio,
            [FromQuery(Name = "fechaHasta")] DateTime? fechaFin
        )
        {
            var oportunidades = await _crmService.GetOportunidadesArchivadas(fechaInicio, fechaFin);
            return Ok(oportunidades);
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
        public async Task<ActionResult<EstadoCRM>> UpdateDescripcion(uint codigo, [FromBody] UpdateEstadoDescripcionDTO request)
        {

            if( request.Descripcion == null)
            {
                return BadRequest("La descripción no puede ser nula");
            }
            var estado = await _estadoCRMRepository.UpdateDescripcion(codigo, request.Descripcion);
            return Ok(estado);
        }

        // Endpoints para Agendamientos
        [HttpGet("agendamientos")]
        public async Task<ActionResult<IEnumerable<AgendamientosCRMViewModel>>> GetAgendamientos()
        {
            var agendamientos = await _agendamientoCRMRepository.GetAllComplete();
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

        [HttpPost("files")]
        public async Task<ActionResult<string>> UploadFile(
            [FromForm] IFormFile file,
            [FromQuery] string folder = "crm"
        )
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado ningún archivo");

            try
            {
                using var stream = file.OpenReadStream();
                var path = await _fileStorageService.SaveFileAsync(stream, file.FileName, folder);
                return Ok(new { path, fileName = file.FileName, size = file.Length });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // NUEVO ENDPOINT PARA LISTAR ARCHIVOS - CORREGIDO
        [HttpGet("files")]
        public ActionResult<IEnumerable<object>> ListFiles([FromQuery] string folder = "crm")
        {
            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "files", folder.ToLowerInvariant());
                
                if (!Directory.Exists(folderPath))
                {
                    return Ok(new List<object>());
                }

                var files = Directory.GetFiles(folderPath)
                    .Select(filePath => new
                    {
                        fileName = Path.GetFileName(filePath),
                        path = Path.Combine(folder, Path.GetFileName(filePath)).Replace("\\", "/"),
                        size = new FileInfo(filePath).Length,
                        lastModified = System.IO.File.GetLastWriteTime(filePath),
                        contentType = GetContentType(Path.GetFileName(filePath))
                    })
                    .OrderByDescending(f => f.lastModified);

                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("files/{*path}")]
        public async Task<ActionResult> GetFile(string path)
        {
            try
            {
                // Validar que el path no termine en / (carpeta)
                if (path.EndsWith("/") || path.EndsWith("\\"))
                {
                    return BadRequest("Debe especificar un archivo, no una carpeta");
                }

                var fileStream = await _fileStorageService.GetFileAsync(path);
                var fileName = Path.GetFileName(path);
                var contentType = GetContentType(fileName);
                
                return File(fileStream, contentType, fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("Archivo no encontrado");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("files/{*path}")]
        public async Task<ActionResult> DeleteFile(string path)
        {
            try
            {
                await _fileStorageService.DeleteFileAsync(path);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        public interface IColaboradoresDTO
        {
            public uint Proyecto { get; set; }
            public IEnumerable<uint> Colaboradores { get; set; }
        }

        [HttpPost("colaboradores")]
        public async Task<ActionResult<IEnumerable<ProyectosColaboradoresCRM>>> CreateColaboradores([FromBody] IColaboradoresDTO request)
        {
            try
            {
                var colaboradoresCreados = await _crmService.CreateColaborador(request.Proyecto, request.Colaboradores);
                return Ok(colaboradoresCreados);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("colaboradores")]
        public async Task<ActionResult<ProyectosColaboradoresCRM>> UpdateColaborador([FromBody] ProyectosColaboradoresCRM colaborador)
        {
            var colaboradorActualizado = await _proyectosColaboradoresRepository.Update(colaborador);
            return Ok(colaboradorActualizado);
        }
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }

        // endpoints para pestanas
        [HttpGet("pestanas")]
        public async Task<ActionResult<IEnumerable<PestanaCRM>>> GetPestanas()
        {
            var pestanas = await _crmService.GetPestanas();
            return Ok(pestanas);
        }

        [HttpPost("pestanas")]
        public async Task<ActionResult<PestanaCRM>> CreatePestana([FromBody] PestanaCRM pestana)
        {
            var pestanaCreada = await _crmService.CrearPestana(pestana);
            return Ok(pestanaCreada);
        }

        [HttpPut("pestanas")]
        public async Task<ActionResult<PestanaCRM>> UpdatePestana([FromBody] PestanaCRM pestana)
        {
            var pestanaActualizada = await _crmService.ActualizarPestana(pestana);
            return Ok(pestanaActualizada);
        }

        [HttpGet("pestanas/tareas/{pestana}")]
        public async Task<ActionResult<IEnumerable<TareaDinamicaCRM>>> GetTareasByPestana(uint pestana)
        {
            var tareas = await _crmService.GetTareasByPestana(pestana);
            return Ok(tareas);
        }

        [HttpPost("pestanas/tareas")]
        public async Task<ActionResult<TareaDinamicaCRM>> CreateTareaDinamica([FromBody] TareaDinamicaCRM tarea)
        {
            var tareaCreada = await _crmService.CrearTareaDinamica(tarea);
            return Ok(tareaCreada);
        }

        [HttpPut("pestanas/tareas")]
        public async Task<ActionResult<TareaDinamicaCRM>> UpdateTareaDinamica([FromBody] TareaDinamicaCRM tarea)
        {
            var tareaActualizada = await _crmService.ActualizarTareaDinamica(tarea);
            return Ok(tareaActualizada);
        }

        [HttpGet("pestanas/oportunidades/{pestana}")]
        public async Task<ActionResult<IEnumerable<OportunidadViewModel>>> GetOportunidadesByPestana(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            uint pestana
        )
        {
            var oportunidades = await _crmService.GetOportunidadesByPestana(fechaInicio, fechaFin, pestana);
            return Ok(oportunidades);
        }

        [HttpPost("pestanas/oportunidades")]
        public async Task<ActionResult<OportunidadPestanaCRM>> CreateOportunidadPestana([FromBody] OportunidadPestanaCRM oportunidadPestana)
        {
            var oportunidadPestanaCreada = await _crmService.CrearOportunidadPestana(oportunidadPestana);
            return Ok(oportunidadPestanaCreada);
        }   

        [HttpDelete("pestanas/oportunidades")]
        public async Task<ActionResult<bool>> DeleteOportunidadPestana(uint oportunidad, uint pestana)
        {
            var oportunidadPestanaEliminada = await _crmService.EliminarOportunidadPestana(oportunidad, pestana);
            return Ok(oportunidadPestanaEliminada);
        }   
    }
}
