using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("agenda_subvisitas")]
    public class AgendaSubvisita
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_cliente")]
        public int? IdCliente { get; set; }
        [Column("id_agenda")]
        public uint IdAgenda { get; set; }
        [Column("nombre_cliente")]
        public string? NombreCliente { get; set; }
        [Column("motivo_visita")]
        public string? MotivoVisita { get; set; }
        [Column("resultado_visita")]
        public string? ResultadoVisita { get; set; }
    }
}