using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("compras")]
    public class Compra
    {
        [Key]
        [Column("co_codigo")]
        public uint Codigo { get; set; }

        [Column("co_fecha")]
        public DateTime Fecha { get; set; }

        [Column("co_proveedor")]
        public uint? Proveedor { get; set; }

        [Column("co_deposito")]
        public uint? Deposito { get; set; }

        [Column("co_moneda")]
        public uint Moneda { get; set; }

        [Column("co_operador")]
        public uint Operador { get; set; }

        [Column("co_factura")]
        [StringLength(20)]
        public string? Factura { get; set; }

        [Column("co_timbrado")]
        [StringLength(20)]
        public string Timbrado { get; set; } = string.Empty;

        [Column("co_vencimiento")]
        public DateTime? Vencimiento { get; set; }

        [Column("co_descuento")]
        public decimal Descuento { get; set; } = 0.00m;

        [Column("co_credito")]
        public byte Credito { get; set; } = 0;

        [Column("co_saldo")]
        public decimal Saldo { get; set; } = 0.00m;

        [Column("co_estado")]
        public byte Estado { get; set; } = 1;

        [Column("co_total")]
        public decimal? Total { get; set; }

        [Column("co_cuotas")]
        public byte Cuotas { get; set; } = 0;

        [Column("co_cantCuotas")]
        public uint CantidadCuotas { get; set; } = 0;

        [Column("co_obs")]
        [StringLength(120)]
        public string Observaciones { get; set; } = string.Empty;

        [Column("co_legal")]
        public byte Legal { get; set; } = 0;

        [Column("co_sucursal")]
        public uint Sucursal { get; set; } = 1;

        [Column("co_metodo")]
        public byte Metodo { get; set; } = 1;

        [Column("co_aplicar_a")]
        public byte AplicarA { get; set; } = 0;

        [Column("co_retencion")]
        public byte Retencion { get; set; } = 0;

        [Column("co_orden")]
        public uint Orden { get; set; } = 0;

        [Column("co_hora")]
        [StringLength(20)]
        public string Hora { get; set; } = string.Empty;

        [Column("co_userpc")]
        [StringLength(60)]
        public string UserPc { get; set; } = string.Empty;

        [Column("co_situacion")]
        public byte Situacion { get; set; } = 1;

        [Column("co_secuencia")]
        public uint Secuencia { get; set; } = 0;

        [Column("co_tipo_compra")]
        public byte TipoCompra { get; set; } = 0;

        [Column("co_sector")]
        public uint Sector { get; set; } = 0;

        [Column("co_tipodoc")]
        public uint TipoDocumento { get; set; } = 1;

        [Column("co_verificado")]
        public int Verificado { get; set; } = 0;

        [Column("co_responsable_ubicacion")]
        public int ResponsableUbicacion { get; set; } = 0;

        [Column("co_verificador")]
        public int Verificador { get; set; } = 0;

        [Column("co_confirmador")]
        public int Confirmador { get; set; } = 0;
        
        public virtual ICollection<DetalleCompra>? Items { get; set; }
    }
}