using System;

namespace Api.Models.Dtos.Articulo
{
    public class ArticuloEnPedidoResponse
    {
        public int IdDetallePedido { get; set; }
        public string Fecha { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public int Tipo { get; set; }
    }
}
