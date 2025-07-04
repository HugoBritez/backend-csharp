using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Entities
{
    [Table("pedidos")]
    public class Pedido
    {
        [Key]
        [Column("p_codigo")]
        public uint Codigo { get; set; }
        [Column("p_fecha")]
        public DateTime Fecha { get; set; }
        [Column("p_nropedido")]
        public string NroPedido { get; set; } = string.Empty;
        [Column("p_cliente")]
        public uint Cliente { get; set; }
        [Column("p_operador")]
        public uint Operador { get; set; }
        [Column("p_moneda")]
        public uint Moneda { get; set; }
        [Column("p_deposito")]
        public uint Deposito { get; set; }
        [Column("p_sucursal")]
        public uint Sucursal { get; set; }
        [Column("p_descuento")]
        public decimal Descuento { get; set; }
        [Column("p_obs")]
        public string Observacion { get; set; } = string.Empty;
        [Column("p_estado")]
        public int Estado { get; set; }
        [Column("p_vendedor")]
        public uint Vendedor { get; set; }
        [Column("p_area")]
        public uint Area { get; set; }
        [Column("p_tipo_estado")]
        public uint TipoEstado { get; set; }
        [Column("p_credito")]
        public uint Credito { get; set; }
        [Column("p_imprimir")]
        public int Imprimir { get; set; }
        [Column("p_interno")]
        public string Interno { get; set; } = string.Empty;
        [Column("p_latitud")]
        public string Latitud { get; set; } = string.Empty;
        [Column("p_longitud")]
        public string Longitud { get; set; } = string.Empty;
        [Column("p_tipo")]
        public int Tipo { get; set; }
        [Column("p_entrega")]
        public decimal Entrega { get; set; }
        [Column("p_cantcuotas")]
        public uint CantCuotas { get; set; }
        [Column("p_consignacion")]
        public uint Consignacion { get; set; }
        [Column("p_autorizar_a_contado")]
        public uint AutorizarContado { get; set; }
        [Column("p_zona")]
        public int Zona { get; set; }
        [Column("p_acuerdo")]
        public int Acuerdo { get; set; }
        [Column("p_imprimir_preparacion")]
        public int ImprimirPreparacion { get; set; }
        [Column("p_cantidad_cajas")]
        public int CantidadCajas { get; set; }
        [Column("p_preparado_por")]
        public int PreparadoPor { get; set; }
        [Column("p_verificado_por")]
        public int VerificadoPor { get; set; }
    }
}