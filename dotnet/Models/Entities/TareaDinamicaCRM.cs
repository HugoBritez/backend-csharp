using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("tareas_dinamicas_crm")]
    public class TareaDinamicaCRM
    {
        [Key]
        [Column("td_codigo")]
        public uint Codigo { get; set; }
        [Column("td_titulo")]
        public string? Titulo { get; set;}
        [Column("td_descripcion")]
        public string? Descripcion { get; set; }
        [Column("td_fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Column("td_fecha_limite")]
        public DateTime? FechaLimite { get; set; }
        [Column("td_fecha_finalizacion")]
        public DateTime FechaFinalizacion { get; set; }
        [Column("td_estado")]
        public int Estado { get; set; } = 1;
        [Column("td_completado")]
        public int Completado { get; set; } = 0;
        [Column("td_creador")]
        public uint Creador { get; set; }
        [Column("td_pestana")]
        public uint Pestana { get; set; }
        [Column("td_operador")]
        public uint? Operador { get; set; }
        [Column("td_proyecto")]
        public uint? Proyecto { get; set; }
        [Column("td_contacto")]
        public uint? Contacto { get; set; }
        [Column("td_public")]
        public int Public { get; set; } = 1;
    }
}