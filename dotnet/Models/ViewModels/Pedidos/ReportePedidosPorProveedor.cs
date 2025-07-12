namespace Api.Models.ViewModels
{
    public class ReportePedidosPorProveedor
    {
        public uint PedidoId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string ClienteRuc { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Factura { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public string Operador { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Condicion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public uint EstadoNum { get; set; }
        public decimal TotalItems { get; set; }
        public decimal TotalImporte { get; set; }
        public string Deposito { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string SiguienteArea { get; set; } = string.Empty;
        public uint CodigoProveedor { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public string Obs { get; set; } = string.Empty;
        public uint CantCuotas { get; set; }
        public decimal Entrega { get; set; }
        public string Acuerdo { get; set; } = string.Empty;
    }
} 