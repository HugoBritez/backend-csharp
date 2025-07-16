using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamento>> GetAll(string? busqueda);
        Task<Departamento?> GetById(uint id);
    }
}