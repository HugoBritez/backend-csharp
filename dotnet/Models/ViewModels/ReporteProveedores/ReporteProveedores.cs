namespace Api.Models.ViewModels
{
    public class ReporteProveedores
    {
        public uint CodigoProducto { get;set;}
        public string DescripcionProducto { get; set; } = string.Empty;
        public decimal TotalStock { get; set; }
        public decimal TotalItems { get; set; }
        public decimal TotalImporte { get; set;}
        public decimal MontoCobrado { get; set;}
        public decimal TotalCompras { get; set;}
    }
}