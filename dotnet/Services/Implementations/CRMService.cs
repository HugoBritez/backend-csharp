using Api.Model.Entities;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Api.Services.Implementations
{
    public class CRMService : ICRMService
    {
        private readonly IContactosCRMRepository _contactosCRMRepository;
        private readonly IOportunidadesCRMRepository _oportunidadesCRMRepository;
        private readonly ITareasCRMRepository _tareasCRMRepository;
        private readonly ILogger<CRMService> _logger;
        private readonly IZonaRepository _zonasRepository;
        private readonly IDepartamentoRepository _departamentosRepository;
        private readonly ICiudadesRepository _ciudadesRepository;

        public CRMService(
            IContactosCRMRepository contactosCRMRepository, 
            IOportunidadesCRMRepository oportunidadesCRMRepository, 
            ITareasCRMRepository tareasCRMRepository,
            ILogger<CRMService> logger,
            IZonaRepository zonasRepository,
            IDepartamentoRepository departamentosRepository,
            ICiudadesRepository ciudadesRepository)
        {
            _contactosCRMRepository = contactosCRMRepository;
            _oportunidadesCRMRepository = oportunidadesCRMRepository;
            _tareasCRMRepository = tareasCRMRepository;
            _logger = logger;
            _zonasRepository = zonasRepository;
            _departamentosRepository = departamentosRepository;
            _ciudadesRepository = ciudadesRepository;
        }

        #region Contactos
        public async Task<ContactoCRM> CrearContacto(ContactoCRM contacto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo contacto: {Nombre}", contacto.Nombre);
                var resultado = await _contactosCRMRepository.CrearContacto(contacto);
                _logger.LogInformation("Contacto creado exitosamente con ID: {Id}", resultado.Codigo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear contacto: {Nombre}", contacto.Nombre);
                throw;
            }
        }

        public async Task<ContactoCRM> ActualizarContacto(ContactoCRM contacto)
        {
            try
            {
                _logger.LogInformation("Actualizando contacto con ID: {Id}", contacto.Codigo);
                var resultado = await _contactosCRMRepository.ActualizarContacto(contacto);
                _logger.LogInformation("Contacto actualizado exitosamente: {Id}", resultado.Codigo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar contacto con ID: {Id}", contacto.Codigo);
                throw;
            }
        }

        public async Task<ContactoViewModel?> GetContactoById(uint id)
        {
            try
            {
                _logger.LogDebug("Buscando contacto con ID: {Id}", id);
                return await _contactosCRMRepository.GetContactoCompletoById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contacto con ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ContactoViewModel>> GetContactos()
        {
            try
            {
                _logger.LogDebug("Obteniendo lista de contactos");
                return await _contactosCRMRepository.GetContactosCompletos();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de contactos");
                throw;
            }
        }
        #endregion

        #region Oportunidades
        public async Task<OportunidadCRM> CrearOportunidad(OportunidadCRM oportunidad)
        {
            try
            {
                _logger.LogInformation("Creando nueva oportunidad: {Titulo}", oportunidad.Titulo);
                var resultado = await _oportunidadesCRMRepository.CrearOportunidad(oportunidad);
                _logger.LogInformation("Oportunidad creada exitosamente con ID: {Id}", resultado.Codigo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear oportunidad: {Titulo}", oportunidad.Titulo);
                throw;
            }
        }

        public async Task<OportunidadCRM> ActualizarOportunidad(OportunidadCRM oportunidad)
        {
            try
            {
                _logger.LogInformation("Actualizando oportunidad con ID: {Id}", oportunidad.Codigo);
                var resultado = await _oportunidadesCRMRepository.ActualizarOportunidad(oportunidad);
                _logger.LogInformation("Oportunidad actualizada exitosamente: {Id}", resultado.Codigo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar oportunidad con ID: {Id}", oportunidad.Codigo);
                throw;
            }
        }

        public async Task<OportunidadViewModel?> GetOportunidadById(uint id)
        {
            try
            {
                _logger.LogDebug("Buscando oportunidad con ID: {Id}", id);
                return await _oportunidadesCRMRepository.GetOportunidadCompletaById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener oportunidad con ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidades(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                _logger.LogDebug("Obteniendo lista de oportunidades con filtros: fechaInicio={FechaInicio}, fechaFin={FechaFin}", 
                    fechaInicio?.ToString("yyyy-MM-dd"), fechaFin?.ToString("yyyy-MM-dd"));
                return await _oportunidadesCRMRepository.GetOportunidadesCompletas(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de oportunidades");
                throw;
            }
        }

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidadesByCliente(uint cliente)
        {
            try
            {
                _logger.LogDebug("Obteniendo oportunidades para cliente: {Cliente}", cliente);
                return await _oportunidadesCRMRepository.GetOportunidadesCompletasByCliente(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener oportunidades para cliente: {Cliente}", cliente);
                throw;
            }
        }

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidadesByOperador(uint operador)
        {
            try
            {
                _logger.LogDebug("Obteniendo oportunidades para operador: {Operador}", operador);
                return await _oportunidadesCRMRepository.GetOportunidadesCompletasByOperador(operador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener oportunidades para operador: {Operador}", operador);
                throw;
            }
        }
        #endregion

        #region Tareas
        public async Task<TareaCRM> CrearTarea(TareaCRM tarea)
        {
            try
            {
                _logger.LogInformation("Creando nueva tarea: {Titulo}", tarea.Titulo);
                var resultado = await _tareasCRMRepository.CrearTarea(tarea);
                _logger.LogInformation("Tarea creada exitosamente con ID: {Id}", resultado.Codigo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear tarea: {Titulo}", tarea.Titulo);
                throw;
            }
        }

        public async Task<TareaCRM> ActualizarTarea(TareaCRM tarea)
        {
            try
            {
                _logger.LogInformation("Actualizando tarea con ID: {Id}", tarea.Codigo);
                var resultado = await _tareasCRMRepository.ActualizarTarea(tarea);
                _logger.LogInformation("Tarea actualizada exitosamente: {Id}", resultado.Codigo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar tarea con ID: {Id}", tarea.Codigo);
                throw;
            }
        }

        public async Task<TareaCRM?> GetTareaById(uint id)
        {
            try
            {
                _logger.LogDebug("Buscando tarea con ID: {Id}", id);
                return await _tareasCRMRepository.GetTareaById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tarea con ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TareaCRM>> GetTareas()
        {
            try
            {
                _logger.LogDebug("Obteniendo lista de tareas");
                return await _tareasCRMRepository.GetTareas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de tareas");
                throw;
            }
        }

        public async Task<IEnumerable<TareaCRM>> GetTareasByOportunidad(uint oportunidad)
        {
            try
            {
                _logger.LogDebug("Obteniendo tareas para oportunidad: {Oportunidad}", oportunidad);
                return await _tareasCRMRepository.GetTareasByOportunidad(oportunidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas para oportunidad: {Oportunidad}", oportunidad);
                throw;
            }
        }

        public async Task<IEnumerable<TareaCRM>> GetTareasByOperador(uint operador)
        {
            try
            {
                _logger.LogDebug("Obteniendo tareas para operador: {Operador}", operador);
                return await _tareasCRMRepository.GetTareasByOperador(operador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas para operador: {Operador}", operador);
                throw;
            }
        }

        public async Task<IEnumerable<TareaCRM>> GetTareasByContacto(uint contacto)
        {
            try
            {
                _logger.LogDebug("Obteniendo tareas para contacto: {Contacto}", contacto);
                return await _tareasCRMRepository.GetTareasByContacto(contacto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas para contacto: {Contacto}", contacto);
                throw;
            }
        }
        #endregion

        #region Tipos de Tareas
        public async Task<IEnumerable<TipoTareaCRM>> GetTiposTareas()
        {
            try
            {
                _logger.LogDebug("Obteniendo tipos de tareas");
                return await _tareasCRMRepository.GetTiposTareas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de tareas");
                throw;
            }
        }
        #endregion
    }
}