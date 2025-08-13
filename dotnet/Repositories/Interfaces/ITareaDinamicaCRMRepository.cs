using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface ITareaDinamicaRepository
    {
        Task<IEnumerable<TareaDinamicaCRM>> ListarTareasPorPestana(uint idPestana);
        Task<IEnumerable<TareaDinamicaCRM>> GetAllTareas();

        Task<TareaDinamicaCRM> CreateTareaDinamica(TareaDinamicaCRM data);

        Task<TareaDinamicaCRM> UpdateTarea(TareaDinamicaCRM data);
    }
}