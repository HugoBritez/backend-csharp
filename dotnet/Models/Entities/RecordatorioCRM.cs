using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("recordatorios_crm")]
    public class RecordatorioCRM
    {
        [Key]
        [Column("re_codigo")]
        public uint Codigo { get; set;}
        [Column("re_titulo")]
        public string Titulo { get; set;} = string.Empty;
        [Column("re_descripcion")]
        public string Descripcion { get; set;} = string.Empty;
        [Column("re_fecha")]
        public DateTime Fecha { get; set;}
        [Column("re_fecha_limite")]
        public DateTime FechaLimite { get; set;}
        [Column("re_hora")]
        public TimeSpan Hora { get; set;}
        [Column("re_operador")]
        public uint Operador { get; set;}
        [Column("re_cliente")]
        public uint Cliente { get; set;}
        [Column("re_estado")]
        public int Estado { get; set;}
        [Column("re_tipo_recordatorio")]
        public uint TipoRecordatorio { get; set;}
        [Column("re_enviado")]
        public int Enviado { get; set;} = 0;
    }
}