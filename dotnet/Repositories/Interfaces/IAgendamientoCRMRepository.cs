using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Implementations
{
    public interface IAgendamientoCRMRepository
    {
        Task<IEnumerable<AgendamientoCRM>> GetAll();
        Task<IEnumerable<AgendamientoCRM>> GetByOperador(uint operador);
        Task<IEnumerable<AgendamientoCRM>> GetByDoctor(uint doctor);
        Task<AgendamientoCRM?> GetById(uint id);
        Task<AgendamientoCRM> Create(AgendamientoCRM agendamiento);
        Task<AgendamientoCRM> Update(AgendamientoCRM agendamiento);    
        Task<IEnumerable<AgendamientosCRMViewModel>> GetAllComplete();     
    }
}