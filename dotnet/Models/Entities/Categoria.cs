using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("ca_codigo")]
        public uint CaCodigo { get; set; }

        [Column("ca_descripcion")]
        public string CaDescripcion { get; set; } = string.Empty;
    }
}
