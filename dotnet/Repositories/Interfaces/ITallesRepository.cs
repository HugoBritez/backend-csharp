using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface ITallesRepository
    {
        Task<IEnumerable<Talle>> GetAll(); 
    }
}