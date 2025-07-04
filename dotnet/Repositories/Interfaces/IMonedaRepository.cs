using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IMonedaRepository
    {
        Task<IEnumerable<Moneda>> GetAll();
    }
}
