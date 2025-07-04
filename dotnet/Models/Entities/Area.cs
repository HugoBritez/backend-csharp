using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("area")]
    public class Area
    {
        [Key]
        [Column("a_codigo")]
        public uint Id { get; set; }

        [Column("a_descripcion")]
        public string? Descripcion { get; set; }
    }
}