using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IUbicacionesRepository
    {
        Task<IEnumerable<Ubicacion>> GetUbicaciones(string? busqueda);
        Task<Ubicacion?> GetById(uint id);
    }
}
