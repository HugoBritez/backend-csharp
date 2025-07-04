using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("localizacion")]
    public class Localizacion
    {
        [Key]
        [Column("l_codigo")]
        public uint Id { get; set; }
        [Column("l_agenda")]
        public uint Agenda { get; set; }
        [Column("l_fecha")]
        public DateTime Fecha { get; set; }
        [Column("l_hora_inicio")]
        public string? HoraInicio { get; set; }
        [Column("l_hora_fin")]
        public string? HoraFin { get; set; }
        [Column("l_obs")]
        public string? Obs { get; set; }
        [Column("l_Cliente")]
        public uint Cliente { get; set; }
        [Column("l_operador")]
        public uint Operador { get; set; }
        [Column("l_Longitud")]
        public string? Longitud { get; set; }
        [Column("l_latitud")]
        public string? Latitud { get; set; }
        [Column("l_acuracia")]
        public decimal Acuraci { get; set; }
        [Column("l_estado")]
        public int Estado { get; set; }
    }
}