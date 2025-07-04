using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("iva")]
    public class Iva
    {
        [Key]
        [Column("iva_codigo")]
        public uint IvaCodigo { get; set; }

        [Column("iva_descripcion")]
        public string IvaDescripcion { get; set; } = string.Empty;
    }
}