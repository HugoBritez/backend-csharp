using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("tipo_doc_identificador")]
    public class TipoDocumento
    {
        [Key]
        [Column("t_codigo")]
        public uint Id { get; set; }
        [Column("t_descripcion")]
        public string? Descripcion { get; set; }
        [Column("t_nro")]
        public int Nro { get; set; }
        [Column("t_estado")]
        public int Estado { get; set; }
    }
}