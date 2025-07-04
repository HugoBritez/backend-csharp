using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Dtos
{
    public class DetallePedidoPatchDTO
    {
        public uint? Codigo  { get; set; }
        public uint? Articulo { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Exentas { get; set; }
        public decimal? Cinco { get; set; }
        public decimal? Diez { get; set; }
        public string? Lote { get; set; } = string.Empty;
        public DateOnly? Vencimiento { get; set; }
        public uint? Vendedor { get; set; }
        public uint? CodigoLote { get; set; }
        public int? Facturado { get; set; }
        public decimal? PorComision { get; set; }
        public uint? Actorizado { get; set; }
        public uint? Habilitar { get; set; }
        public uint? Bonificacion { get; set; }
        public string? DescripcionArticulo { get; set; } = string.Empty;
        public string? Observacion { get; set; } = string.Empty;
        public int? CantidadCargada { get; set; }
    } 
}