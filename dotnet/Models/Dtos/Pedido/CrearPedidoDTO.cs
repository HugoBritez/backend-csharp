using Api.Models.Entities;

namespace Api.Models.Dtos
{
    public class CrearPedidoDTO
    {
        public required Pedido Pedido { get; set; }
        public required IEnumerable<DetallePedido> DetallePedido { get; set; }
    }
}