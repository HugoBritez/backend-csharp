using Api.Model.Entities;
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

        Task<OportunidadCRM> CrearOportunidad(OportunidadCRM oportunidad);

        Task<OportunidadCRM> ActualizarOportunidad(OportunidadCRM oportunidad);

        Task<OportunidadViewModel?> GetOportunidadById(uint id);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidades();

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
    }
}