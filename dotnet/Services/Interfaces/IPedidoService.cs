using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IPedidosService
    {
        Task<Pedido> CrearPedido(Pedido pedido, IEnumerable<DetallePedido> detallePedido);
        Task<string> AnularPedido(uint codigo, string motivo);
        Task<IEnumerable<PedidoViewModel>> GetPedidos(
            string? fechaDesde,
            string? fechaHasta,
            string? nroPedido,
            int? articulo,
            string? clientes,
            string? vendedores,
            string? sucursales,
            string? estado,
            int? moneda,
            string? factura
        );

        Task<ResponseViewModel<Pedido>> AutorizarPedido(uint idPedido, string usuario, string password);
    }
}