using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IMetaVentaRepository
    {
        Task<MetaVentaArticulo> CrearMeta(MetaVentaArticulo data);

        Task<MetaVentaArticulo?> GetMetaPorArticulo(uint IdArticulo);

        Task<IEnumerable<MetaVentaArticulo>> GetMetasEnPeriodo(int anio);
    }
}