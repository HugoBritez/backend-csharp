using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetallePedidoFaltanteRepository
    {
        Task<DetallePedidoFaltante> Crear(DetallePedidoFaltante detalle);

        Task<DetallePedidoFaltante?> GetById(uint id);

        Task<DetallePedidoFaltante> Update(DetallePedidoFaltante detalle);

        Task<DetallePedidoFaltante?> GetByPedido(uint pedido);
    }
}