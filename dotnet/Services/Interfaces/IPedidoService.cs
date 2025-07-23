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
            IEnumerable<int>? clientes,
            string? vendedores,
            string? sucursales,
            string? estado,
            int? moneda,
            string? factura,
            int? limit = null
        );

        Task<ResponseViewModel<Pedido>> AutorizarPedido(uint idPedido, string usuario, string password);

        Task<DetallePedido?> CambiarLoteDetallePedido(uint idDetallePedido, string lote, uint idLote);

        Task<DetallePedidoFaltante> AnularFaltante(uint detalleFaltante);

        Task<IEnumerable<PedidoViewModel>> GetPedidosPorCliente(string clienteRuc);
    }
}