using Api.Model.Entities;
using Api.Models.Dtos.CRM;
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
        private readonly IProyectosColaboradoresRepositoryCRM _proyectosColaboradoresRepository;
        public CRMService(
            IContactosCRMRepository contactosCRMRepository, 
            IOportunidadesCRMRepository oportunidadesCRMRepository, 
            ITareasCRMRepository tareasCRMRepository,
            ILogger<CRMService> logger,
            IZonaRepository zonasRepository,
            IDepartamentoRepository departamentosRepository,
            ICiudadesRepository ciudadesRepository,
            IProyectosColaboradoresRepositoryCRM proyectosColaboradoresRepository)
        {
            _contactosCRMRepository = contactosCRMRepository;
            _oportunidadesCRMRepository = oportunidadesCRMRepository;
            _tareasCRMRepository = tareasCRMRepository;
            _logger = logger;
            _zonasRepository = zonasRepository;
            _departamentosRepository = departamentosRepository;
            _ciudadesRepository = ciudadesRepository;
            _proyectosColaboradoresRepository = proyectosColaboradoresRepository;
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
        public async Task<OportunidadCRM> CrearOportunidad(CrearOportunidadDTO oportunidad)
        {
            try
            {
                _logger.LogInformation("Creando nueva oportunidad: {Titulo}", oportunidad.Titulo);
                var resultado = await _oportunidadesCRMRepository.CrearOportunidad(oportunidad);
                _logger.LogInformation("Oportunidad creada exitosamente con ID: {Id}", resultado.Codigo);
                if (oportunidad.Colaboradores != null && oportunidad.Colaboradores.Any())
                {
                    await CreateColaborador(oportunidad.Codigo, oportunidad.Colaboradores);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear oportunidad: {Titulo}", oportunidad.Titulo);
                throw;
            }
        }

        public async Task<OportunidadCRM> ActualizarOportunidad(CrearOportunidadDTO oportunidad)
        {
            try
            {
                _logger.LogInformation("Actualizando oportunidad con ID: {Id}", oportunidad.Codigo);
                var resultado = await _oportunidadesCRMRepository.ActualizarOportunidad(oportunidad);
                _logger.LogInformation("Oportunidad actualizada exitosamente: {Id}", resultado.Codigo);
                
                // Sincronizar colaboradores usando soft delete
                if (oportunidad.Colaboradores != null)
                {
                    await SincronizarColaboradores(oportunidad.Codigo, oportunidad.Colaboradores);
                }
                else
                {
                    // Si la lista es null, desactivar todos los colaboradores
                    await _proyectosColaboradoresRepository.SoftDeleteByProyecto(oportunidad.Codigo);
                    _logger.LogInformation("Todos los colaboradores desactivados del proyecto {Proyecto}", oportunidad.Codigo);
                }
                
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
                var oportunidad = await _oportunidadesCRMRepository.GetOportunidadCompletaById(id);
                if (oportunidad != null)
                {
                    var colaboradores = await _proyectosColaboradoresRepository.GetByProyecto(id);
                    oportunidad.Colaboradores = colaboradores;
                }
                return oportunidad;
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
                var oportunidades = await _oportunidadesCRMRepository.GetOportunidadesCompletas(fechaInicio, fechaFin);
                foreach (var oportunidad in oportunidades)
                {
                    var colaboradores = await _proyectosColaboradoresRepository.GetByProyecto(oportunidad.Codigo);
                    oportunidad.Colaboradores = colaboradores;
                }
                return oportunidades;
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

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidadesArchivadas(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _oportunidadesCRMRepository.GetOportunidadesArchivadas(fechaInicio, fechaFin);
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

        #region Colaboradores
        public async Task<IEnumerable<ProyectosColaboradoresCRM>> CreateColaborador(uint proyecto, IEnumerable<uint> colaboradores)
        {
            try
            {
                _logger.LogInformation("Creando colaboradores para proyecto: {Proyecto}", proyecto);
                
                if (colaboradores == null || !colaboradores.Any())
                {
                    throw new ArgumentException("La lista de colaboradores no puede ser nula o vacía");
                }

                var colaboradoresCreados = new List<ProyectosColaboradoresCRM>();
                
                foreach (var colaborador in colaboradores)
                {
                    // Verificar si ya existe el colaborador activo para este proyecto
                    var existente = await _proyectosColaboradoresRepository.GetByProyecto(proyecto);
                    if (existente.Any(c => c.Colaborador == colaborador && c.Estado == 1))
                    {
                        _logger.LogWarning("El colaborador {Colaborador} ya existe activo para el proyecto {Proyecto}", colaborador, proyecto);
                        continue;
                    }

                    // Verificar si existe pero está inactivo para reactivarlo
                    var inactivo = existente.FirstOrDefault(c => c.Colaborador == colaborador && c.Estado == 0);
                    if (inactivo != null)
                    {
                        await _proyectosColaboradoresRepository.ActivarColaborador(proyecto, colaborador);
                        colaboradoresCreados.Add(inactivo);
                        _logger.LogDebug("Colaborador {Colaborador} reactivado en el proyecto {Proyecto}", colaborador, proyecto);
                        continue;
                    }

                    var nuevoColaborador = new ProyectosColaboradoresCRM
                    {
                        Proyecto = proyecto,
                        Colaborador = colaborador,
                        Estado = 1
                    };

                    var creado = await _proyectosColaboradoresRepository.Create(nuevoColaborador);
                    colaboradoresCreados.Add(creado);
                    _logger.LogDebug("Colaborador {Colaborador} agregado al proyecto {Proyecto}", colaborador, proyecto);
                }

                _logger.LogInformation("Se procesaron {Cantidad} colaboradores para el proyecto {Proyecto}", 
                    colaboradoresCreados.Count, proyecto);
                
                return colaboradoresCreados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear colaboradores para proyecto: {Proyecto}", proyecto);
                throw;
            }
        }
        
        public async Task<IEnumerable<ProyectosColaboradoresCRM>> SincronizarColaboradores(uint proyecto, IEnumerable<uint> nuevosColaboradores)
        {
            try
            {
                _logger.LogInformation("Sincronizando colaboradores para proyecto: {Proyecto}", proyecto);
                
                // Obtener colaboradores activos actuales
                var colaboradoresActuales = await _proyectosColaboradoresRepository.GetByProyecto(proyecto);
                var colaboradoresActivosIds = colaboradoresActuales.Where(c => c.Estado == 1).Select(c => c.Colaborador).ToHashSet();
                
                // Convertir la nueva lista a HashSet para comparación eficiente
                var nuevosColaboradoresSet = nuevosColaboradores?.ToHashSet() ?? new HashSet<uint>();
                
                // Colaboradores a desactivar (están activos pero no en la nueva lista)
                var colaboradoresADesactivar = colaboradoresActivosIds.Except(nuevosColaboradoresSet);
                
                // Colaboradores a activar/agregar (están en la nueva lista pero no activos)
                var colaboradoresAActivar = nuevosColaboradoresSet.Except(colaboradoresActivosIds);
                
                // Desactivar colaboradores que ya no están en la lista
                foreach (var colaboradorId in colaboradoresADesactivar)
                {
                    await _proyectosColaboradoresRepository.SoftDeleteByProyectoAndColaborador(proyecto, colaboradorId);
                    _logger.LogDebug("Colaborador {Colaborador} desactivado del proyecto {Proyecto}", colaboradorId, proyecto);
                }
                
                // Activar/agregar nuevos colaboradores
                var colaboradoresProcesados = new List<ProyectosColaboradoresCRM>();
                foreach (var colaboradorId in colaboradoresAActivar)
                {
                    // Verificar si existe pero está inactivo
                    var existente = colaboradoresActuales.FirstOrDefault(c => c.Colaborador == colaboradorId && c.Estado == 0);
                    if (existente != null)
                    {
                        await _proyectosColaboradoresRepository.ActivarColaborador(proyecto, colaboradorId);
                        colaboradoresProcesados.Add(existente);
                        _logger.LogDebug("Colaborador {Colaborador} reactivado en el proyecto {Proyecto}", colaboradorId, proyecto);
                    }
                    else
                    {
                        // Crear nuevo colaborador
                        var nuevoColaborador = new ProyectosColaboradoresCRM
                        {
                            Proyecto = proyecto,
                            Colaborador = colaboradorId,
                            Estado = 1
                        };

                        var creado = await _proyectosColaboradoresRepository.Create(nuevoColaborador);
                        colaboradoresProcesados.Add(creado);
                        _logger.LogDebug("Colaborador {Colaborador} agregado al proyecto {Proyecto}", colaboradorId, proyecto);
                    }
                }
                
                _logger.LogInformation("Sincronización completada para proyecto {Proyecto}: {Desactivados} desactivados, {Activados} activados/agregados", 
                    proyecto, colaboradoresADesactivar.Count(), colaboradoresAActivar.Count());
                
                return colaboradoresProcesados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sincronizar colaboradores para proyecto: {Proyecto}", proyecto);
                throw;
            }
        }
        #endregion
    }
}