using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("detalle_presupuesto")]
    public class DetallePresupuesto
    {
        [Key]
        [Column("depre_codigo")]
        public uint Codigo { get; set; }
        [Column("depre_presupuesto")]
        public uint Presupuesto { get; set; }
        [Column("depre_articulo")]
        public uint Articulo { get; set; }
        [Column("depre_cantidad")]
        public decimal Cantidad { get; set; }
        [Column("depre_precio")]
        public decimal Precio { get; set; }
        [Column("depre_descuento")]
        public decimal Descuento { get; set; }
        [Column("depre_exentas")]
        public decimal Exentas { get; set; }
        [Column("depre_cinco")]
        public decimal Cinco { get; set; }
        [Column("depre_diez")]
        public decimal Diez { get; set; }
        [Column("depre_porcentaje")]
        public decimal Porcentaje { get; set; }
        [Column("depre_altura")]
        public decimal Altura { get; set; }
        [Column("depre_largura")]
        public decimal Largura { get; set; }
        [Column("depre_mts2")]
        public decimal Mts2 { get; set; }
        [Column("depre_listaprecio")]
        public uint ListaPrecio { get; set; }
        [Column("depre_talle")]
        public string Talle { get; set; } = string.Empty;
        [Column("depre_codlote")]
        public uint CodigoLote { get; set; }
        [Column("depre_lote")]
        public string Lote { get; set; } = string.Empty;
        [Column("depre_vence")]
        public DateOnly Vencimiento { get; set; }
        [Column("depre_descripcio_art")]
        public string DescripcionArticulo { get; set; } = string.Empty;
        [Column("depre_obs")]
        public string Observacion { get; set; } = string.Empty;
        [Column("depre_procesado")]
        public uint Procesado { get; set; }
    }
}