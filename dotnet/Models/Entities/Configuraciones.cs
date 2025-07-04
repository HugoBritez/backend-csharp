using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("configuraciones")]
    public class Configuracion
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Column("valor")]
        public string Valor { get; set; } = string.Empty;
    }
}