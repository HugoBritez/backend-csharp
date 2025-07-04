using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("agendas")]
    public class Agenda
    {
        [Key]
        [Column("a_codigo")]
        public uint Id { get; set; }
        [Column("a_fecha")]
        public DateTime Fecha { get; set; }
        [Column("a_hora")]
        public string? Hora { get; set; }
        [Column("a_dias")]
        public string? Dias { get; set; }
        [Column("a_cliente")]
        public uint Cliente { get; set; }
        [Column("a_operador")]
        public uint Operador { get; set; }
        [Column("a_vendedor")]
        public int Vendedor { get; set; }
        [Column("a_planificacion")]
        public uint Planificacion { get; set; }
        [Column("a_prioridad")]
        public uint Prioridad { get; set; }
        [Column("a_obs")]
        public string? Observacion { get; set; }
        [Column("a_prox_llamada")]
        public DateTime? ProximaLlamada { get; set; }
        [Column("a_hora_prox")]
        public string? HoraProx { get; set; } 
        [Column("a_prox_acti")]
        public string? ProximaActi { get; set; }
        [Column("a_visitado")]
        public int Visitado { get; set; }
        [Column("a_visitado_prox")]
        public int VisitadoProx { get; set; }
        [Column("a_latitud")]
        public string? Latitud { get; set; }
        [Column("a_longitud")]
        public string? Longitud { get; set; }
        [Column("a_estado")]
        public int Estado { get; set; }
    }
}