using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IUnidadMedidaRepository
    {
        Task<IEnumerable<UnidadMedida>> GetUnidadMedidas(string? busqueda);
        Task<UnidadMedida?> GetById(uint id);
    }
}
