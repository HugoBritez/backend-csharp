using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IMetaGeneralRepository
    {
        Task<MetasVentaGeneral> CrearMetaGeneral(MetasVentaGeneral data);

        Task<MetasVentaGeneral?> GetMetaGeneralPorArticulo(uint IdArticulo);

        Task<IEnumerable<MetasVentaGeneral>> GetMetasGeneralEnPeriodo(int anio);
    }
}