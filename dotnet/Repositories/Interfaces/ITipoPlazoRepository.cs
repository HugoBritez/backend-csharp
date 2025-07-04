using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface ITipoPlazoRepository
    {
        Task<IEnumerable<TipoPlazo>> GetAll();
    }
}