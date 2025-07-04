namespace Api.Models.ViewModels
{
    public class Impresionventa
    {
        public int Codigo { get; set; }
        public string? TipoVenta { get; set; }
        public string? FechaVenta { get; set; }
        public string? FechaVencimiento { get; set; }
        public string? Cajero { get; set; }
        public string? Vendedor { get; set; }
        public string? Cliente { get; set; }
        public string? ClienteCorreo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Ruc { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalDescuento { get; set; }
        public decimal TotalAPagar { get; set; }
        public decimal TotalExentas { get; set; }
        public decimal TotalDiez { get; set; }
        public decimal TotalCinco { get; set; }
        public string? Timbrado { get; set; }
        public string? Factura { get; set; }
        public string? FacturaValidoDesde { get; set; }
        public string? FacturaValidoHasta { get; set; }
        public string? VeQr { get; set; }
        public string? VeCdc { get; set; }
        public int UsaFe { get; set; }
        public string? Moneda { get; set; }
        public decimal Cotizacion { get; set; }
        public List<VentaDetalle> Detalles { get; set; } = [];
        public List<SucursalData> SucursalData { get; set; } = [];
        public required List<ConfiguracionFacturaElectronica> ConfiguracionFacturaElectronica { get; set; }
    }

    public class VentaDetalle
    {
        public int Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public decimal Exentas { get; set; }
        public decimal Diez { get; set; }
        public decimal Cinco { get; set; }
        public string? FechaVencimiento { get; set; }
        public string? Lote { get; set; }
        public int ControlVencimiento { get; set; }
    }

    public class SucursalData
    {
        public string? SucursalNombre { get; set; }
        public string? SucursalDireccion { get; set; }
        public string? SucursalTelefono { get; set; }
        public string? SucursalEmpresa { get; set; }
        public string? SucursalRuc { get; set; }
        public string? SucursalMatriz { get; set; }
        public string? SucursalCorreo { get; set; }
    }

    public class ConfiguracionFacturaElectronica
    {
        public string? Nombre { get; set; }
        public string? Fantasia { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Ruc { get; set; }
        public string? Correo { get; set; }
        public string? DescripcionEstablecimiento { get; set; }
        public string? DatoEstablecimiento { get; set; }
    }

}