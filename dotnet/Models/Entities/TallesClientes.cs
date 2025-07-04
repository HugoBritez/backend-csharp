using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("cliente_talle")]
    public class TalleCliente
    {
        [Key]
        [Column("c_codigo")]
        public uint Codigo { get; set; }
        [Column("c_cliente")]
        public uint ClienteId { get; set; }
        [Column("c_talle")]
        public uint TalleId { get; set; }
        [Column("c_nrotalle")]
        public uint NroTalle { get; set; }
        [Column("c_tipo")]
        public int Tipo { get; set; } = 0;

        [ForeignKey("TalleId")]
        public Talle? Talle { get; set; }
    }
}