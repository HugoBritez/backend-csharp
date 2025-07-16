using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("oportunidades_crm")]
    public class OportunidadCRM
    {
        [Key]
        [Column("op_codigo")]
        public uint Codigo { get; set; }
        [Column("op_cliente")]
        public uint Cliente { get; set; }
        [Column("op_titulo")]
        public string? Titulo { get; set; }
        [Column("op_descripcion")]
        public string? Descripcion { get; set; }
        [Column("op_valor_negociacion")]
        public decimal ValorNegociacion { get; set; }
        [Column("op_fecha_inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("op_fecha_fin")]
        public DateTime? FechaFin { get; set; }
        [Column("op_operador")]
        public uint Operador { get; set; }
        [Column("op_estado")]
        public uint Estado { get; set; }
        [Column("op_general")]
        public int General { get; set; }
    }
}