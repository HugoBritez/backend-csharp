using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("cuentasbco")]
    public class CuentaBancaria
    {
        [Key]
        [Column("cb_codigo")]
        public uint Codigo { get; set;}
        [Column("cb_banco")]
        public uint Banco { get; set;}
        [Column("cb_descripcion")]
        public string? Descripcion { get; set; }
        [Column("cb_fecha")]
        public DateTime Fecha { get; set;}
        [Column("cb_moneda")]
        public uint Moneda { get; set; }
        [Column("cb_titular")]
        public uint Titular { get; set;}
        [Column("cb_contacto")]
        public string? Contacto { get; set;}
        [Column("cb_mail")]
        public string? Mail { get; set; }
        [Column("cb_tel")]
        public string? Telefono { get; set;}
        [Column("cb_obs")]
        public string? Observacion { get; set; }
        [Column("cb_saldo")]
        public decimal Saldo { get; set;}
        [Column("cb_estado")]
        public int Estado { get; set; }
        [Column("cb_aplicar")]
        public uint Aplicar { get; set;}
        [Column("cb_tipocuenta")]
        public uint TipoCuenta { get; set; }
        [Column("cb_plan")]
        public uint Plan { get; set;}
    }
}