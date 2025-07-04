using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("metas_venta_articulos")]
    public class MetaVentaArticulo
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        [Column("ar_codigo")]
        public uint ArticuloId { get; set; }
        [Column("op_codigo")]
        public uint OperadorId { get; set; }
        [Column("meta_acordada")]
        public decimal MetaAcordada { get; set; }
        [Column("periodo")]
        public int Periodo { get; set; }
        [Column("estado")]
        public int Estado { get; set; }

    }
}