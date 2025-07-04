using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces{
    public interface IControlIngresoRepository
    {
        Task<IEnumerable<CompraConsultaViewModel>> GetFacturas(
            uint? deposito,
            uint? proveedor,
            DateTime? fechadesde,
            DateTime? fechahasta,
            string? factura,
            uint? verificado
        );

        Task<IEnumerable<DetalleCompraConsultaViewModel>> GetItems(
            uint idCompra,
            string? busqueda,
            bool aVerificar
        );

        Task<IEnumerable<ReporteIngresosViewModel>> Reporte(
            uint? deposito,
            uint? proveedor,
            DateTime? fechadesde,
            DateTime? fechahasta,
            string? factura,
            uint? verificado
        );
    }
    
}
