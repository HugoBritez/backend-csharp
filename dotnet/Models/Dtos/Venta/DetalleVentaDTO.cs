
using System.Text.Json.Serialization;

namespace Api.Models.Dtos
{
    public class DetalleVentaDTO
    {
        public uint DeveVenta { get; set; }
        public uint DeveArticulo { get; set; }
        public decimal DeveCantidad { get; set; }
        public decimal DevePrecio { get; set; }
        public decimal DeveDescuento { get; set; }
        public decimal DeveExentas { get; set; }
        public decimal DeveCinco { get; set; }
        public decimal DeveDiez { get; set; }
        public int DeveDevolucion { get; set; }
        public uint DeveVendedor { get; set; }
        public string? DeveColor { get; set; }
        public uint DeveBonificacion { get; set; }
        public string? DeveTalle { get; set; }
        public uint DeveCodioot { get; set; }
        public decimal DeveCosto { get; set; }
        public decimal DeveCostoArt { get; set; }
        public decimal DeveCincoX { get; set; }
        public decimal DeveDiezX { get; set; }
        public string? Lote { get; set; }
        [JsonPropertyName("loteId")]
        public int LoteId { get; set; }
        public bool ArticuloEditado { get; set; }
        public string? DeveDescripcionEditada { get; set; }
        public string? CodigoBarras { get; set; }
    }
}