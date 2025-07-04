using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IMetodoPagoRepository
    {
        Task<IEnumerable<MetodoPago>> GetAll();
    }
}
