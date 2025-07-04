using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("depositos")]
    public class Deposito
    {
        [Key]
        [Column("dep_codigo")]
        public uint DepCodigo { get; set; }

        [Column("dep_descripcion")]
        public string? DepDescripcion { get; set; }

        [Column("dep_obs")]
        public string? DepObservaciones { get; set; }

        [Column("dep_estado")]
        public byte DepEstado { get; set; } = 1;

        [Column("dep_sucursal")]
        public uint DepSucursal { get; set; } = 1;

        [ForeignKey("DepSucursal")]
        public virtual Sucursal? Sucursal { get; set; }

        [Column("dep_origen")]
        public uint DepOrigen { get; set; } = 1;

        [Column("dep_subdeposito")]
        public byte DepSubDeposito { get; set; } = 0;

        [Column("dep_autorizacion")]
        public byte DepAutorizacion { get; set; } = 0;

        [Column("dep_plan")]
        public uint DepPlan { get; set; } = 0;

        [Column("dep_principal")]
        public byte DepPrincipal { get; set; } = 0;

        [Column("dep_atencion")]
        public uint DepAtencion { get; set; } = 0;

        [Column("dep_interno")]
        public byte DepInterno { get; set; } = 0;

        [Column("dep_inventario")]
        public byte DepInventario { get; set; } = 0;

        [InverseProperty("Deposito")]  // Add this attribute
        public virtual ICollection<ArticuloLote>? ArticulosLote { get; set; }
    }
}
