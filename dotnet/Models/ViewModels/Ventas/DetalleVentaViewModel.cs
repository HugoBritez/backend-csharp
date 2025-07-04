namespace Api.Models.ViewModels
{
    public class DetalleVentaViewModel
    {
        public uint det_codigo { get; set; }
        public uint art_codigo { get; set; }
        public string codbarra { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public string descripcion_editada { get; set; } = string.Empty;
        public string cantidad { get; set; } = string.Empty;
        public string precio { get; set; } = string.Empty;
        public decimal precio_number { get; set; }
        public string descuento { get; set; } = string.Empty;
        public decimal descuento_number { get; set; }
        public string exentas { get; set; } = string.Empty;
        public decimal exentas_number { get; set; }
        public string cinco { get; set; } = string.Empty;
        public decimal cinco_number { get; set; }
        public string diez { get; set; } = string.Empty;
        public decimal diez_number { get; set; }
        public string lote { get; set; } = string.Empty;
        public string vencimiento { get; set; } = string.Empty;
        public string largura { get; set; } = string.Empty;
        public string altura { get; set; } = string.Empty;
        public string mt2 { get; set; } = string.Empty;
        public string kilos { get; set; } = string.Empty;
        public uint unidad_medida { get; set; }
    }
}