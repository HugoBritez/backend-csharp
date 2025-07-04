using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("area_secuencia")]
    public class AreaSecuencia
    {
        [Key]
        [Column("ac_codigo")]
        public uint Codigo { get; set; }
        [Column("ac_area")]
        public uint Area { get; set; }
        [Column("ac_secuencia_area")]
        public uint SecuenciaArea { get; set; }
    }
}