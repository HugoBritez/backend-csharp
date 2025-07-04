using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("talles")]
    public class Talle
    {
        [Key]
        [Column("t_codigo")]
        public uint Codigo { get; set; }
        [Column("t_descripcion")]
        public string? Descripcion { get; set; }
        [Column("t_tipo")]
        public int Tipo { get; set; } = 0;
        [Column("t_estado")]
        public int Estado { get; set; } = 1;
        [Column("t_abreviado")]
        public string? Abreviado { get; set; }
        [Column("t_desde")]
        public int Desde { get; set; } = 0;
        [Column("t_hasta")]
        public int Hasta { get; set; } = 0;
    }
}