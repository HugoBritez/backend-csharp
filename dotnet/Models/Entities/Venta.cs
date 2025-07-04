using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("ventas")]
    public class Venta
    {
        [Key]
        [Column("ve_codigo")]
        public uint Codigo { get; set; }
        [Column("ve_cliente")]
        public uint Cliente { get; set; }

        [Column("ve_operador")]
        public uint Operador { get; set; }

        [Column("ve_deposito")]
        public uint Deposito { get; set; }
        [Column("ve_moneda")]
        public uint Moneda { get; set; }
        [Column("ve_fecha")]
        public DateTime Fecha { get; set; }

        [Column("ve_factura")]
        public string Factura { get; set; } = string.Empty;

        [Column("ve_credito")]
        public uint Credito { get; set; }

        [Column("ve_saldo")]
        public decimal Saldo { get; set; }

        [Column("ve_total")]
        public decimal Total { get; set; }

        [Column("ve_vencimiento")]
        public DateTime Vencimiento { get; set; }

        [Column("ve_estado")]
        public uint Estado { get; set; }

        [Column("ve_devolucion")]
        public uint Devolucion { get; set; }

        [Column("ve_procesado")]
        public uint Procesado { get; set; }

        [Column("ve_descuento")]
        public decimal Descuento { get; set; }

        [Column("ve_cuotas")]
        public uint Cuotas { get; set; }

        [Column("ve_cantCuotas")]
        public uint CantCuotas { get; set; }

        [Column("ve_obs")]
        public string Obs { get; set; } = string.Empty;

        [Column("ve_vendedor")]
        public uint Vendedor { get; set; }

        [Column("ve_sucursal")]

        public uint Sucursal { get; set; }

        [Column("ve_metodo")]
        public int Metodo { get; set; }

        [Column("ve_aplicar_a")]
        public int AplicarA { get; set; }

        [Column("ve_retencion")]
        public int Retencion { get; set; }

        [Column("ve_timbrado")]
        public string Timbrado { get; set; } = string.Empty;

        [Column("ve_codeudor")]
        public uint Codeudor { get; set; }

        [Column("ve_pedido")]
        public int Pedido { get; set; }

        [Column("ve_hora")]
        public string Hora { get; set; } = string.Empty;

        [Column("ve_userpc")]
        public string UserPc { get; set; } = string.Empty;

        [Column("ve_situacion")]
        public uint Situacion { get; set; }

        [Column("ve_chofer")]
        public uint Chofer { get; set; }

        [Column("ve_cdc")]
        public string Cdc { get; set; } = string.Empty;

        [Column("ve_qr")]
        public string Qr { get; set; } = string.Empty;

        [Column("ve_km_actual")]
        public uint KmActual { get; set; }

        [Column("ve_vehiculo")]
        public uint Vehiculo { get; set; }

        [Column("ve_desc_trabajo")]
        public string DescTrabajo { get; set; } = string.Empty;

        [Column("ve_kilometraje")]
        public int Kilometraje { get; set; }

        [Column("ve_servicio")]
        public uint Servicio { get; set; }

        [Column("ve_siniestro")]
        public string Siniestro { get; set; } = string.Empty;


    }
}