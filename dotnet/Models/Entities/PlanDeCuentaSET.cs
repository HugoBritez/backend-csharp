using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("plan_de_cuenta_set")]
    public class PlanDeCuentaSET
    {
        [Key]
        [Column("p_codigo")]
        public int Codigo { get; set;}

        [Column("p_titulo")]
        public string? Titulo { get; set; }
        [Column("p_asentable")]
        public int Asentable { get; set;}
        [Column("p_nivel")]
        public int Nivel { get; set; }
        [Column("p_clasificion")]
        public int Clasificacion { get; set; }
        [Column("p_observaciones")]
        public string? Observaciones { get; set; }
        [Column("p_codificacion")]
        public string? Codificacion { get; set; }
        [Column("p_naturaliza")]
        public int Naturaleza { get; set; }
        [Column("p_cal_f_renta")]
        public uint Renta { get; set; }
        [Column("p_aplicativo")]
        public uint Aplicativo { get; set;}
        [Column("p_apli_gasto")]
        public uint Gasto { get; set; }
        [Column("p_moneda")]
        public uint Moneda { get; set; }
        [Column("p_tipo_cambio")]
        public uint TipoCambio { get; set;}
        [Column("p_estado")]
        public int Estado { get; set; }
    }
}