namespace Api.Models.ViewModels
{
    public class ReporteVentaPorProveedorViewModel
    {
        public IEnumerable<ReporteProveedores> ReporteVentasPorProveedor { get; set; } = [];
        public decimal TotalCompras { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Saldo { get; set; }
    }
}