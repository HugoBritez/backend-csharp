using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("detalle_pedido")]
    public class DetallePedido
    {
        [Key]
        [Column("dp_codigo")]
        public uint Codigo  { get; set; }
        [Column("dp_pedido")]
        public uint Pedido { get; set; }
        [Column("dp_articulo")]
        public uint Articulo { get; set; }
        [Column("dp_cantidad")]
        public decimal Cantidad { get; set; }
        [Column("dp_precio")]
        public decimal Precio { get; set; }
        [Column("dp_descuento")]
        public decimal Descuento { get; set; }
        [Column("dp_exentas")]
        public decimal Exentas { get; set; }
        [Column("dp_cinco")]
        public decimal Cinco { get; set; }
        [Column("dp_diez")]
        public decimal Diez { get; set; }
        [Column("dp_lote")]
        public string Lote { get; set; } = string.Empty;
        [Column("dp_vence")]
        public DateOnly Vencimiento { get; set; }
        [Column("dp_vendedor")]
        public uint Vendedor { get; set; }
        [Column("dp_codigolote")]
        public uint CodigoLote { get; set; }
        [Column("dp_facturado")]
        public int Facturado { get; set; }
        [Column("dp_porcomision")]
        public decimal PorComision { get; set; }
        [Column("dp_actorizado")]
        public uint Actorizado { get; set; }
        [Column("dp_habilitar")]
        public uint Habilitar { get; set; }
        [Column("dp_bonif")]
        public uint Bonificacion { get; set; }
        [Column("dp_descripcion_art")]
        public string DescripcionArticulo { get; set; } = string.Empty;
        [Column("dp_obs")]
        public string Observacion { get; set; } = string.Empty;
        [Column("cantidad_cargada")]
        public int CantidadCargada { get; set; }
    } 
}