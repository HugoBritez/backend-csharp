using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("ordenes_compra")]
    public class OrdenCompra
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        
        [Column("fecha")]
        public DateTime Fecha { get; set; }
        
        [Column("fechaEntrega")]
        public DateTime FechaEntrega { get; set; }
        
        [Column("proveedor")]
        public uint Proveedor { get; set; }
        
        [Column("operador")]
        public uint Operador { get; set; }
        
        [Column("chofer")]
        public uint Chofer { get; set; }
        
        [Column("comprobante")]
        [StringLength(20)]
        public string Comprobante { get; set; } = string.Empty;
        
        [Column("estado")]
        public byte Estado { get; set; }
        
        [Column("moneda")]
        public byte Moneda { get; set; } = 0;
        
        [Column("confirmado")]
        public byte Confirmado { get; set; } = 0;
        
        [Column("concepto")]
        [StringLength(200)]
        public string Concepto { get; set; } = string.Empty;
        
        [Column("fechavenc")]
        public DateTime FechaVencimiento { get; set; } = new DateTime(1, 1, 1);
        
        [Column("secuencia")]
        public uint Secuencia { get; set; } = 0;
        
        [Column("condicion")]
        public byte Condicion { get; set; } = 0;
        
        [Column("deposito")]
        public uint Deposito { get; set; } = 1;
        
        [Column("tipo_orden")]
        public byte TipoOrden { get; set; } = 0;
        
        [Column("idOrden_pedido")]
        public uint IdOrdenPedido { get; set; } = 0;
    }
}