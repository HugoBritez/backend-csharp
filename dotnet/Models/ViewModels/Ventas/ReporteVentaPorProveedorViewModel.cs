namespace Api.Models.ViewModels
{
    public class ReporteVentaPorProveedorViewModel
    {
        public IEnumerable<ReporteProveedores> ReporteVentasPorProveedor { get; set; } = [];
        public decimal TotalCompras { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Saldo { get; set; }
        public decimal TotalStock { get; set; }
        public decimal TotalValorizacion { get; set; }
        public decimal TotalUnidadesCompradas { get; set; }
        public decimal TotalImporteDeCompras { get; set; }
        public decimal TotalUnidadesVendidas { get; set; }
        public decimal TotalImporteDeVentas { get; set; }
        public decimal TotalCobrado { get; set; }
        public decimal UtilidadPromedio { get; set; }
    }
}