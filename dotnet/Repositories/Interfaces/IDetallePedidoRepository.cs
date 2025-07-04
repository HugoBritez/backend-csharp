using Api.Models.Entities;
using Api.Models.ViewModels;
namespace Api.Repositories.Interfaces
{
    public interface IDetallePedidoRepository
    {
        Task<DetallePedido> Crear(DetallePedido detalle);
        Task<IEnumerable<PedidoDetalleViewModel>> GetDetallesPedido(Pedido pedido);

        Task<IEnumerable<DetallePedido>> GetByPedido(uint id);
    }
}