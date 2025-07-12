namespace Api.Models.ViewModels
{
    public class ReporteVentasPorProveedor
    {
        public uint CodigoVenta { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string ClienteRuc { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Factura { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public string Operador { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }
        public decimal Saldo { get; set; }
        public string Condicion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal TotalItems { get; set; }
        public decimal TotalImporte { get; set; }
        public decimal MontoCobrado { get; set; }
        public string Deposito { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
        public uint CodigoProveedor { get; set; }
        public string Proveedor { get; set; } = string.Empty;
    }
}