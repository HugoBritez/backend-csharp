using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities{
    [Table("detalle_compras_vencimineto")]
    public class DetalleCompraVencimiento{
        [Key]
        [Column("dv_compra")]
        public uint Compra { get; set; }
        [Key]
        [Column("dv_detalle_compra")]
        public uint DetalleCompra { get; set; }
        [Column("dv_articulo")]
        public uint Articulo { get; set; }
        [Column("dv_lote")]
        public string Lote { get; set; } = string.Empty;
        [Column("dv_vencimiento")]
        public DateTime Vence { get; set; } = new DateTime(1, 1, 1);
        [Column("loteid")]
        public int LoteId { get; set; }
    }
}