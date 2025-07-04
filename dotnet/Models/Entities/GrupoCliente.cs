using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("grupos_clientes")]
    public class GrupoCliente
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}