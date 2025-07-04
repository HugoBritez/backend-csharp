using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("detalle_faltante")]
    public class DetallePedidoFaltante
    {
        [Key]
        [Column("d_codigo")]
        public uint Codigo { get; set; }

        [Column("d_detalle_pedido")]
        public uint DetallePedido { get; set; }
        [Column("d_cantidad")]
        public decimal Cantidad { get; set; }
        [Column("d_situacion")]
        public uint Situacion { get; set; }
        [Column("obs")]
        public string Observacion { get; set; } = string.Empty;        
    }
}