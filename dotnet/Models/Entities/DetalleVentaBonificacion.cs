using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Api.Models.Entities
{
    [Table("detalle_venta_bonificacion")]
    public class DetalleVentaBonificacion
    {
        [Key]
        [Column("d_codigo")]
        public uint Codigo { get; set; }

        [Column("d_detalle_venta")]
        public uint DetalleVenta { get; set; }

        [Column("d_cantidad")]
        public decimal Cantidad { get; set; }

        [Column("d_precio")]
        public decimal Precio { get; set; }

        [Column("d_exentas")]
        public decimal Exentas { get; set; }
        [Column("d_cinco")]
        public decimal Cinco { get; set; }
        [Column("d_diez")]
        public decimal Diez { get; set; }

    }
}