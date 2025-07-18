using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDoctoresRepository
    {
        Task<IEnumerable<Doctor>> GetAll();

        Task<Doctor?> GetById(uint id);
    }
}