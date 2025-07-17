using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IEstadoCRMRepository
    {
        Task<IEnumerable<EstadoCRM>> GetEstados();
        Task<EstadoCRM> UpdateDescripcion(uint codigo, string descripcion);
    }
}