using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("unidadmedidas")]
    public class UnidadMedida
    {
        [Key]
        [Column("um_codigo")]
        public uint Codigo { get; set; }
        [Column("um_descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Column("um_abreviado")]
        public string Abreviado { get; set; } = string.Empty;
        [Column("um_cod_set")]
        public uint CodigoSet { get; set; }
        [Column("um_obs")]
        public string Observacion { get; set; } = string.Empty;
        [Column("um_estado")]
        public int Estado { get; set; }
    }
}