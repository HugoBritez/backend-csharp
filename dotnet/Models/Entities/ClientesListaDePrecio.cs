using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Api.Models.Entities
{
    [Table("clientes_lp")]
    public class ClientesListaDePrecio
    {
        [Key]
        [Column("clp_cliente")]
        public uint ClienteId { get; set; }

        [Key]
        [Column("clp_lp")]
        public uint ListaDePrecioId { get; set; }

        [JsonIgnore]
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
        [JsonIgnore]
        [ForeignKey("ListaDePrecioId")]
        public ListaPrecio? ListaDePrecio { get; set; }
    }
}