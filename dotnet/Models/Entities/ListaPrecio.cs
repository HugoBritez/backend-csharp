using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Api.Models.Entities
{
    [Table("listasprecios")]
    public class ListaPrecio
    {
        [Key]
        [Column("lp_codigo")]
        public uint LpCodigo { get; set; }

        [Column("lp_descripcion")]
        public string LpDescripcion { get; set; } = string.Empty;

        [Column("lp_estado")]
        public uint LpEstado { get; set; }

        [Column("lp_porcentaje")]
        public double LpPorcentaje { get; set; }

        [Column("lp_moneda")]
        public uint LpMoneda { get; set; }
        
        [JsonIgnore]
        public ICollection<ClientesListaDePrecio>? ClientesListaDePrecios { get; set; }
        
    }
}