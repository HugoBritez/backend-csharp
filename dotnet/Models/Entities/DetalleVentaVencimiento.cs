using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Api.Models.Entities
{
    [Table("detalle_ventas_vencimiento")]
    public class DetalleVentaVencimiento
    {
        [Key]
        [Column("id_detalle_venta")]
        public uint DetalleVenta { get; set; }

        [Column("lote")]
        public string Lote { get; set; } = string.Empty;

        [Column("loteid")]
        public int LoteId { get; set; }
    }
}