using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("sucursales")]
    public class Sucursal
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Column("tel")]
        public string Telefono { get; set; } = string.Empty;

        [Column("titular")]
        public string Titular { get; set; } = string.Empty;

        [Column("fechaAlta")]
        public DateTime FechaAlta { get; set; }

        [Column("scriptimpresion")]
        public string ScriptImpresion { get; set; } = string.Empty;

        [Column("scriptreimpresion")]
        public string ScriptReimpresion { get; set; } = string.Empty;

        [Column("nombre_emp")]
        public string NombreEmpresa { get; set; } = string.Empty;

        [Column("ruc_emp")]
        public string RucEmpresa { get; set; } = string.Empty;

        [Column("linea_baja")]
        public string LineaBaja { get; set; } = string.Empty;

        [Column("uni_personal")]
        public byte UnidadPersonal { get; set; } = 1;

        [Column("matriz")]
        public byte Matriz { get; set; } = 0;

        [Column("mov_tipo_caja")]
        public byte MovTipoCaja { get; set; } = 0;

        [Column("scriptimpresionrec")]
        public string ScriptImpresionRecibo { get; set; } = string.Empty;

        [Column("scriptreimpresionrec")]
        public string ScriptReimpresionRecibo { get; set; } = string.Empty;

        [Column("estado")]
        public byte Estado { get; set; }

        public virtual ICollection<Deposito>? Depositos { get; set; }
    }
}
