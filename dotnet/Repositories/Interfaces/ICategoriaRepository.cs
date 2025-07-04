using Api.Models.Dtos.Sucursal;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetCategorias(string? busqueda);
        Task<Categoria?> GetById(uint id);
    }
}
