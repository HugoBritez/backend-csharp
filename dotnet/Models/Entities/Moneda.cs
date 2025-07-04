using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("monedas")]
    public class Moneda
    {
        [Key]
        [Column("mo_codigo")]
        public uint MoCodigo { get; set;}
        
        [Column("mo_descripcion")]
        public string MoDescripcion { get; set;} = string.Empty;

        [Column("mo_obs")]
        public string MoObs { get; set; } = string.Empty;

        [Column("mo_estado")]
        public uint MoEstado { get; set; }
    }
}