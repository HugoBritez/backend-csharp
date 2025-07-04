using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("cotizaciones")]
    public class Cotizacion
    {
        [Key]
        [Column("co_codigo")]
        public uint Id { get; set; }

        [Column("co_fecha")]
        public DateOnly Fecha { get; set; }

        [Column("co_moneda")]
        public uint Moneda { get; set; }
        [Column("co_monto")]
        public decimal Monto { get; set; }
        [Column("co_monto_c")]
        public decimal MontoCompra { get; set; }
    }
}