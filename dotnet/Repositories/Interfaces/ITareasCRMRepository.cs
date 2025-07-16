using Api.Model.Entities;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface ITareasCRMRepository
    {
        Task<TareaCRM> CrearTarea(TareaCRM tarea);

        Task<TareaCRM> ActualizarTarea(TareaCRM tarea);

        Task<TareaCRM?> GetTareaById(uint id);

        Task<IEnumerable<TareaCRM>> GetTareas();

        Task<IEnumerable<TareaCRM>> GetTareasByOportunidad(uint oportunidad);

        Task<IEnumerable<TareaCRM>> GetTareasByContacto(uint contacto);

        Task<IEnumerable<TipoTareaCRM>> GetTiposTareas();
    }
}