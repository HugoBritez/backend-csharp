using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IAgendasNotasRepository
    {
        Task<AgendasNotas> Crear(AgendasNotas nota);
        Task<AgendasNotas> Update(AgendasNotas nota);
        Task<AgendasNotas?> GetById(uint id);
        Task<IEnumerable<AgendasNotas>> GetByAgenda(uint idAgenda);
    }
}
