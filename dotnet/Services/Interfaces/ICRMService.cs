using Api.Model.Entities;
using Api.Models.Dtos.CRM;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface ICRMService
    {
        Task<ContactoCRM> CrearContacto(ContactoCRM contacto);

        Task<ContactoCRM> ActualizarContacto(ContactoCRM contacto);

        Task<ContactoViewModel?> GetContactoById(uint id);

        Task<IEnumerable<ContactoViewModel>> GetContactos();

        Task<OportunidadCRM> CrearOportunidad(CrearOportunidadDTO oportunidad);

        Task<OportunidadCRM> ActualizarOportunidad(CrearOportunidadDTO oportunidad);

        Task<OportunidadViewModel?> GetOportunidadById(uint id);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidades(DateTime? fechaInicio = null, DateTime? fechaFin = null);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesByCliente(uint cliente);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesByOperador(uint operador);

        Task<TareaCRM> CrearTarea(TareaCRM tarea);

        Task<TareaCRM> ActualizarTarea(TareaCRM tarea);

        Task<TareaCRM?> GetTareaById(uint id);

        Task<IEnumerable<TareaCRM>> GetTareas();

        Task<IEnumerable<TareaCRM>> GetTareasByOportunidad(uint oportunidad);

        Task<IEnumerable<TareaCRM>> GetTareasByOperador(uint operador);

        Task<IEnumerable<TareaCRM>> GetTareasByContacto(uint contacto);

        Task<IEnumerable<TipoTareaCRM>> GetTiposTareas();

        Task<IEnumerable<ProyectosColaboradoresCRM>> CreateColaborador(uint proyecto, IEnumerable<uint> colaboradores);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesArchivadas(DateTime? fechaInicio = null, DateTime? fechaFin = null);

        Task<IEnumerable<PestanaCRM>> GetPestanas();

        Task<PestanaCRM> CrearPestana(PestanaCRM pestana);

        Task<PestanaCRM> ActualizarPestana(PestanaCRM pestana);

        Task<IEnumerable<TareaDinamicaCRM>> GetTareasByPestana(uint pestana);

        Task<TareaDinamicaCRM> CrearTareaDinamica(TareaDinamicaCRM tarea);

        Task<TareaDinamicaCRM> ActualizarTareaDinamica(TareaDinamicaCRM tarea);

        Task<OportunidadPestanaCRM> CrearOportunidadPestana(OportunidadPestanaCRM oportunidadPestana);

        Task<bool> EliminarOportunidadPestana(uint oportunidad, uint pestana);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesByPestana(DateTime fechaInicio, DateTime fechaFin, uint pestana);
    }
}