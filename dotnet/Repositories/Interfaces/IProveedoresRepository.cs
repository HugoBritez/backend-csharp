using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces{
    public interface IProveedoresRepository
    {
        Task<IEnumerable<ProveedorViewModel>> GetProveedores(string? busqueda);
        Task<Proveedor?> GetById(uint id);

        Task<Proveedor> CrearProveedor(Proveedor data);
        Task<Proveedor> UpdateProveedor(Proveedor proveedor);

        Task<IEnumerable<Proveedor>> GetAll(string? Busqueda);

        Task<Proveedor?> GetByRuc(string ruc);

        Task<IEnumerable<ReporteProveedores>> GetReporteProveedores(string? fechaDesde, string? fechaHasta, uint? proveedor, uint? cliente);

        Task<decimal> ObtenerTotalStockPorProveedor(uint proveedor);

    }
}
