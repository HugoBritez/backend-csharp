using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("presupuesto")]
    public class Presupuesto
    {
        [Key]
        [Column("pre_codigo")]
        public uint Codigo { get; set; }
        [Column("pre_cliente")]
        public uint Cliente { get; set; }
        [Column("pre_operador")]
        public uint Operador { get; set; }
        [Column("pre_moneda")]
        public uint Moneda { get; set; }
        [Column("pre_fecha")]
        public DateOnly Fecha { get; set; }
        [Column("pre_descuento")]
        public decimal Descuento { get; set; }
        [Column("pre_estado")]
        public int Estado { get; set; }
        [Column("pre_confirmado")]
        public int Confirmado { get; set; }
        [Column("pre_vendedor")]
        public uint Vendedor { get; set; }
        [Column("pre_credito")]
        public int Credito { get; set; }
        [Column("pre_hora")]
        public string Hora { get; set; } = string.Empty;
        [Column("pre_obs")]
        public string Observacion { get; set; } = string.Empty;
        [Column("pre_flete")]
        public string FLete { get; set; } = "15 Dias";
        [Column("pre_plazo")]
        public string Plazo { get; set; } = "15 Dias";
        [Column("pre_validez")]
        public string Validez { get; set; } = "15 Dias";
        [Column("pre_condicion")]
        public string Condicion { get; set; } = "15 Dias";
        [Column("pre_sucursal")]
        public uint Sucursal { get; set; }
        [Column("pre_deposito")]
        public uint Deposito { get; set; }

    }
}