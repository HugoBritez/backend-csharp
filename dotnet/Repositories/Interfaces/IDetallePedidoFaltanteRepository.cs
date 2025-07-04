using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetallePedidoFaltanteRepository
    {
        Task<DetallePedidoFaltante> Crear(DetallePedidoFaltante detalle);
    }
}