using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("marcas")]
    public class Marca
    {
        [Key]
        [Column("ma_codigo")]
        public uint MaCodigo { get; set; }

    [Column("ma_descripcion")]
    public string MaDescripcion { get; set; } = string.Empty;
}
}
