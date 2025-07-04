using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("subcategorias")]
    public class SubCategoria
    {
        [Key]
        [Column("sc_codigo")]
        public uint ScCodigo { get; set; }

        [Column("sc_descripcion")]
        public string ScDescripcion { get; set; } = string.Empty;

        [Column("sc_categoria")]
        public uint ScCategoria { get; set; }

        public virtual Categoria? Categoria { get; set; }
    }
}
