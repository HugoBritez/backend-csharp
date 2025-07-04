using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IMarcaRepository
    {
        Task<IEnumerable<Marca>> GetMarcas(string? busqueda);
        Task<Marca?> GetById(uint id);
    }
}