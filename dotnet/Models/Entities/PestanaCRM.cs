using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("pestanas_crm")]
    public class PestanaCRM
    {
        [Key]
        [Column("pe_codigo")]
        public uint Codigo { get; set; }

        [Column("pe_descripcion")]
        public string? Descripcion { get; set; }
        [Column("pe_estado")]
        public int Estado { get; set; } = 1;
        [Column("pe_fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Column("pe_operador")]
        public uint Operador { get; set; }
    }
}