using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("presupuesto_observacion")]
    public class PresupuestoObservacion
    {
        [Key]
        [Column("presupuesto")]
        public uint Presupuesto { get; set; }
        [Column("observaciones")]
        public string Observacion { get; set; } = string.Empty;
    }
}