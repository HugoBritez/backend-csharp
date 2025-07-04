namespace Api.Models.Dtos.Articulo
{
    public class ConsultaArticulosDto
    {
        public uint CodigoArticulo { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string DescripcionArticulo { get; set; } = string.Empty;
        public string PrecioCompra { get; set; } = string.Empty;
        public string PrecioVenta { get; set; } = string.Empty;
        public string PrecioVentaCredito { get; set; } = string.Empty;
        public string PrecioVentaMostrador { get; set; } = string.Empty;
        public string PrecioVenta4 { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Deposito { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Subcategoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public string StockActual { get; set; } = string.Empty;
        public string StockMinimo { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public string Vencimiento { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
    }
}