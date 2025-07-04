using Api.Models.Entities;
using Api.Models.Dtos.ArticuloLote;
namespace Api.Repositories.Interfaces
{
    public interface ICiudadesRepository
    {
        Task<IEnumerable<Ciudad>> GetAll(string? Busqueda);
    }
}