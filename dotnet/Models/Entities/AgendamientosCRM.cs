using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("agendamientos_crm")]
    public class AgendamientoCRM
    {
        [Key]
        [Column("ag_codigo")]
        public uint Codigo { get; set; }
        [Column("ag_fecha_inicio")]
        public DateTime FechaInicio { get; set;}
        [Column("ag_fecha_agendamiento")]
        public DateTime FechaAgendamiento { get; set;}
        [Column("ag_hora_agendamiento")]
        public TimeSpan HoraAgendamiento { get; set;}
        [Column("ag_titulo")]
        public string? Titulo { get; set;}
        [Column("ag_descripcion")]
        public string? Descripcion { get; set;}
        [Column("ag_doctor")]
        public uint Doctor { get; set;}
        [Column("ag_paciente")]
        public uint Paciente { get; set;}
        [Column("ag_cliente")]
        public uint Cliente { get; set;}
        [Column("ag_operador")]
        public uint Operador { get; set;}
        [Column("ag_estado")]
        public uint Estado { get; set;}
    }
}