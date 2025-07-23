using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("cargos")]
    public class Cargo
    {
        [Key]
        [Column("car_codigo")]
        public uint Codigo { get; set; }
        [Column("car_decripcion")]
        public string? Descripcion { get; set; }
    }
}