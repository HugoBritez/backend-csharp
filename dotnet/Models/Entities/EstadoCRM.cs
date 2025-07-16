using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("estados_crm")]
    public class EstadoCRM
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}