using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("detalle_compras")]
    public class DetalleCompra
    {
        [Key]
        [Column("dc_id")]
        public uint Id { get; set; }
        
        [Column("dc_compra")]
        public uint Compra { get; set; }
        
        [Column("dc_articulo")]
        public uint Articulo { get; set; }
        
        [Column("dc_cantidad")]
        public decimal Cantidad { get; set; } = 0.0000m;
        
        [Column("dc_precio")]
        public decimal Precio { get; set; } = 0.0000m;
        
        [Column("dc_descuento")]
        public decimal Descuento { get; set; } = 0.00m;
        
        [Column("dc_recargo")]
        public decimal Recargo { get; set; } = 0.00m;
        
        [Column("dc_exentas")]
        public decimal Exentas { get; set; } = 0.00m;
        
        [Column("dc_cinco")]
        public decimal Cinco { get; set; } = 0.00m;
        
        [Column("dc_diez")]
        public decimal Diez { get; set; } = 0.00m;
        
        [Column("dc_cb")]
        public byte Cb { get; set; } = 0;
        
        [Column("dc_lote")]
        [StringLength(25)]
        public string Lote { get; set; } = string.Empty;
        
        [Column("dc_vence")]
        public DateTime Vence { get; set; } = new DateTime(1, 1, 1);
        
        [Column("dc_cantidad_verificada")]
        public int CantidadVerificada { get; set; } = 0;
        
        // Navigation property
        [ForeignKey("Compra")]
        public virtual Compra? CompraNavigation { get; set; }
    }
}