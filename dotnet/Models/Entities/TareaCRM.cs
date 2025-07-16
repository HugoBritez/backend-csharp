using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("tareas_crm")]
    public class TareaCRM
    {
        [Key]
        [Column("ta_codigo")]
        public uint Codigo { get; set; }
        [Column("ta_titulo")]
        public string? Titulo { get; set; }
        [Column("ta_descripcion")]
        public string? Descripcion { get; set; }
        [Column("ta_resultado")]
        public string? Resultado { get; set; }
        [Column("ta_fecha")]
        public DateTime Fecha { get; set; }
        [Column("ta_oportunidad")]
        public uint Oportunidad { get; set; }
        [Column("ta_tipo_tarea")]
        public uint TipoTarea { get; set; }
        [Column("ta_fecha_limite")]
        public DateTime? FechaLimite { get; set; }
        [Column("ta_estado")]
        public int Estado { get; set;}
    }
}