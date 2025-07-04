using Api.Models.Entities;

namespace Api.Models.Dtos
{
    public class CrearVentaDTO
    {
        public required VentaDTO Venta { get; set; } 
        public required  IEnumerable<DetalleVentaDTO> DetalleVenta { get; set; }
    }
}