using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IPacientesRepository
    {
        Task<IEnumerable<Paciente>> GetAll();
        Task<Paciente?> GetById(uint id);
    }
}