using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("bancos")]
    public class Banco
    {
        [Key]
        [Column("ba_codigo")]
        public uint Codigo { get; set; }
        [Column("ba_descripcion")]
        public string? Descripcion { get; set; }
        [Column("ba_ciudad")]
        public uint? Ciudad { get; set;}
        [Column("ba_dir")]
        public string? Direccion { get; set; }
        [Column("ba_tel")]
        public string? Telefono { get; set; }
        [Column("ba_obs")]
        public string? Observacion { get; set;}
        [Column("ba_estado")]
        public int Estado { get; set; }
    }
}