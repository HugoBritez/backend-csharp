using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("sub_ubicacion")]
    public class Sububicacion
    {
        [Key]
        [Column("s_codigo")]
        public uint SCodigo { get; set; }

        [Column("s_descripcion")]
        public string SDescripcion { get; set; } = string.Empty;

        [Column("s_ubicacion")]
        public uint SUbicacion { get; set; }
    }
}
