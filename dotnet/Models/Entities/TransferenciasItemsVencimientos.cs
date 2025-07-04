using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("transferencias_items_vencimiento")]
    public class TransferenciaItemVencimiento
    {
        [Key]
        [Column("tiv_id")]
        public uint Id { get; set; }
        [Column("tiv_id_ti")]
        public uint IdItem { get; set; }
        [Column("tiv_lote")]
        public string? Lote { get; set; }
        [Column("date_lote")]
        public DateTime Fecha { get; set; }
        [Column("loteid")]
        public int LoteId { get; set; }
        [Column("loteidd")]
        public int LoteIDD { get; set; }
    }
}