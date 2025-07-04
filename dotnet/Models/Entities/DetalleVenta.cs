using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Api.Models.Entities
{
    [Table("detalle_ventas")]
    public class DetalleVenta
    {
        [Key]
        [Column("deve_codigo")]
        public uint Codigo { get; set; }

        [Column("deve_venta")]
        public uint Venta { get; set; }

        [Column("deve_articulo")]
        public uint Articulo { get; set; }

        [Column("deve_cantidad")]
        public decimal Cantidad { get; set; }

        [Column("deve_precio")]
        public decimal Precio { get; set; }

        [Column("deve_descuento")]
        public decimal Descuento { get; set; }

        [Column("deve_exentas")]
        public decimal Exentas { get; set; }

        [Column("deve_cinco")]
        public decimal Cinco { get; set; }

        [Column("deve_diez")]
        public decimal Diez { get; set; }

        [Column("deve_devolucion")]
        public int Devolucion { get; set; }

        [Column("deve_vendedor")]
        public uint Vendedor { get; set; }

        [Column("deve_bonificacion")]
        public uint Bonificacion { get; set; }

        [Column("deve_talle")]
        public string Talle { get; set; } = string.Empty;

        [Column("deve_codioot")]
        public uint CodigoOT { get; set; }

        [Column("deve_costo")]
        public decimal Costo { get; set; }

        [Column("deve_costo_art")]
        public decimal CostoArt { get; set; }

        [Column("deve_cinco_x")]
        public decimal CincoX { get; set; }
        [Column("deve_diez_x")]
        public decimal DiezX { get; set; }

        [ForeignKey("Venta")]
        public  Venta? VentaNavigation { get; set; }
    }
}