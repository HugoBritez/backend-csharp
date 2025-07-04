using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface ITipoDocumentoRepository
    {
        Task<IEnumerable<TipoDocumento>> GetAll();
    }
}