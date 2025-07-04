using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IZonaRepository
    {
        Task<IEnumerable<Zona>> GetAll();
    }
}