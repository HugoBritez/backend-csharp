using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ICotizacionRepository
    {
        Task<IEnumerable<Cotizacion>> GetCotizacionesHoy();

        Task<Cotizacion?> GetCotizacionDolarHoy();
    }
}
