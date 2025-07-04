using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IControlIngresoService
    {
        Task<bool> VerificarCompra(uint idCompra, uint userId);
        Task<bool> VerificarItem(uint detalle, decimal cantidad, string lote, DateTime vencimiento);
        Task<bool> ConfirmarVerificacion(
            uint idCompra,
            string factura,
            uint deposito_inicial,
            uint deposito_destino,
            uint user_id,
            uint confirmador_id,
            IEnumerable<ItemConfirmarVerificacionDTO> items
        );
    }
}