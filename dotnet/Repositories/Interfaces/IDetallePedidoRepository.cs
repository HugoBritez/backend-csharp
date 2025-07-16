using Api.Models.Entities;
using Api.Models.ViewModels;
namespace Api.Repositories.Interfaces
{
    public interface IDetallePedidoRepository
    {
        Task<DetallePedido> Crear(DetallePedido detalle);
        Task<IEnumerable<PedidoDetalle>> GetDetallesPedido(uint codigo);
        Task<IEnumerable<PedidoDetalle>> GetDetallesPedidoPorProveedor(uint codigo, uint proveedor);

        Task<IEnumerable<DetallePedido>> GetByPedido(uint id);

        Task<DetallePedido?> GetById(uint id);

        Task<DetallePedido> Update(DetallePedido detalle);
    }
}