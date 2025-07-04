using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("zonas")]
    public class Zona
    {
        [Key]
        [Column("zo_codigo")]
        public uint Codigo { get; set; }
        [Column("zo_descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Column("zo_obs")]
        public string Observacion { get; set; } = string.Empty;
        [Column("zo_estado")]
        public int Estado { get; set; }

        public virtual ICollection<Proveedor> Proveedores { get; set; } = new HashSet<Proveedor>();

    }
}