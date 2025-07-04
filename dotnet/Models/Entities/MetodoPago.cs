using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("metodospago")]
    public class MetodoPago
    {
        [Key]
        [Column("me_codigo")]
        public uint Id { get; set; }
        [Column("me_descripcion")]
        public string? Descripcion { get; set; }
        [Column("me_estado")]
        public int Estado { get; set; }
    }
}