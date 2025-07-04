using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("ciudades")]
    public class Ciudad
    {
        [Key]
        [Column("ciu_codigo")]
        public uint Id { get; set; }
        [Column("ciu_cod_interno")]
        public string? CodigoInterno { get; set; }
        [Column("ciu_descripcion")]
        public string? Descripcion { get; set; }
        [Column("ciu_departamento")]
        public uint Departamento { get; set; }
        [Column("ciu_distrito")]
        public uint Distrito { get; set; }
        [Column("ciu_estado")]
        public int Estado { get; set; }
    }
}