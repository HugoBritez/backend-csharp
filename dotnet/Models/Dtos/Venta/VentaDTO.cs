using Api.Models.Entities;

namespace Api.Models.Dtos
{
    public class VentaDTO : Venta
    {
        public uint? CajaDefinicion { get; set; }
        public uint? ConfOperacion { get; set; }
    }
}