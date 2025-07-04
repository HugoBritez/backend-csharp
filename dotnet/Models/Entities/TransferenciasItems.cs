using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("transferencias_items")]
    public class TransferenciaItem
    {
        [Key]
        [Column("ti_id")]
        public uint Id { get; set; }
        
        [Column("ti_transferencia")]
        public uint Transferencia { get; set; }
        
        [Column("ti_articulo")]
        public uint Articulo { get; set; }
        
        [Column("ti_cantidad")]
        public decimal Cantidad { get; set; }
        
        [Column("ti_stock_actuald")]
        public decimal StockActualDestino { get; set; } = 0.00m;
        
        // Navigation properties
        [ForeignKey("Transferencia")]
        public virtual Transferencia? TransferenciaNavigation { get; set; }
    }
}