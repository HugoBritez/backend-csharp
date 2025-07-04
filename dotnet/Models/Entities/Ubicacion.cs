using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("ubicaciones")]
    public class Ubicacion
    {
        [Key]
        [Column("ub_codigo")]
        public uint UbCodigo { get; set; }

        [Column("ub_descripcion")]
        public string UbDescripcion { get; set; } = string.Empty;

        [Column("ub_estado")]
        public byte UbEstado { get; set; } = 1;

    }
}