using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("agendas_notas")]
    public class AgendasNotas
    {
        [Key]
        [Column("an_codigo")]
        public uint Id { get; set; }
        [Column("an_agenda_id")]
        public uint AgendaId { get; set; }
        [Column("an_nota")]
        public string? Nota { get; set; }
        [Column("an_fecha")]
        public DateTime Fecha { get; set; }
        [Column("an_hora")]
        public TimeSpan? Hora { get; set; }
        [Column("an_sistema")]
        public uint Sistema { get; set; }
    }
}