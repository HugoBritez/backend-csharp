using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces{
    public interface IAgendaSubvisitaRepository
    {
        Task<AgendaSubvisita> Crear(AgendaSubvisita data);
        Task<IEnumerable<AgendaSubvisita?>> GetByAgenda(uint agendaId);
        Task<AgendaSubvisita?> GetById(uint id);
        Task<AgendaSubvisita> Update(AgendaSubvisita update);   
    }
}
