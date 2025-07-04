using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("departamentos")]
    public class Departamento
    {
        [Key]
        [Column("dep_codigo")]
        public uint Id { get; set; }
        [Column("dep_descripcion")]
        public string Descripcion { get; set; } = "";
        [Column("dep_estado")]
        public int Estado { get; set; }
    }
}