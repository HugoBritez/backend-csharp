using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("pedidos_estados")]
    public class PedidosEstados
    {
        [Key]
        [Column("pe_codigo")]
        public uint Codigo { get; set; }
        [Column("pe_pedido")]
        public uint Pedido { get; set; }
        [Column("pe_area")]
        public uint Area { get; set; }
        [Column("pe_operador")]
        public string Operador { get; set; } = string.Empty;
        [Column("pe_fecha_urev")]
        public DateTime Fecha { get; set; } 
    }
}