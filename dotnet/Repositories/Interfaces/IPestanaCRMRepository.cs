using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IPestanaCRMRepository
    {
        Task<IEnumerable<PestanaCRM>> ListarPestanas();

        Task<PestanaCRM> CrearPestana(PestanaCRM datos);

        Task<PestanaCRM> UpdatePestana(PestanaCRM datos);
    }
}