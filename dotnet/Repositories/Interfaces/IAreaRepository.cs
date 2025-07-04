using Api.Models.Entities;
using Api.Models.Dtos.ArticuloLote;
namespace Api.Repositories.Interfaces
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAll();
    }
}