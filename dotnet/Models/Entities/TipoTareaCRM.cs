using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.Entities
{
    [Table("tipo_tarea_crm")]
    public class TipoTareaCRM
    {
        [Key]
        [Column("tipo_codigo")]
        public uint Codigo { get; set; }
        [Column("tipo_descripcion")] // reunion personal, llamada, email, conversacion whatsapp etc
        public string? Descripcion { get; set; }
        [Column("tipo_estado")] 
        public uint Estado { get; set;}
    }
}