using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IPedidoEstadoRepository
    {
        Task<PedidosEstados> Crear(PedidosEstados pedidoEstado);
    }
}