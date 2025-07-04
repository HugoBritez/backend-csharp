using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("tipo_plazo")]
    public class TipoPlazo 
    {
        [Key]
        [Column("p_codigo")]
        public uint Codigo { get;set;}
        [Column("p_descripcion")]
        public string? Descripcion { get;set;}
        [Column("p_nro_plazo")]
        public uint NroPlazo { get; set; }
        [Column("p_estado")]
        public int Estado { get;set;}
    }
}