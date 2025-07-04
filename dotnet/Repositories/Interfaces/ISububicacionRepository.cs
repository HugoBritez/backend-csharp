using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ISububicacionRepository
    {
        Task<IEnumerable<Sububicacion>> GetSubUbicaciones(string? busqueda);
        Task<Sububicacion?> GetById(uint id);
    }
}
