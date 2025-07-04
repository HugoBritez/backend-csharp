namespace Api.Models.ViewModels
{
    public class ReporteIngresosViewModel
    {
        public uint IdCompra { get; set; }
        public string FechaCompra { get; set; } = string.Empty;
        public uint Deposito { get; set; }
        public string DepositoDescripcion { get; set; } = string.Empty;
        public string? NroFactura { get; set; }
        public uint IdOrden { get; set; }
        public uint NroProveedor { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public uint ProveedorId { get; set; }
        public int Verificado { get; set; }
        public string? ResponsableUbicacion { get; set; }
        public string? ResponsableVerificacion { get; set; }
        public string? ResponsableConfirmacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<ReporteIngresosItemViewModel> Items { get; set; } = new List<ReporteIngresosItemViewModel>();
    }

    public class ReporteIngresosItemViewModel
    {
        public uint DetalleCompra { get; set; }
        public uint ArticuloId { get; set; }
        public string ArticuloDescripcion { get; set; } = string.Empty;
        public string? ArticuloCodigoBarras { get; set; }
        public decimal Cantidad { get; set; }
        public int CantidadVerificada { get; set; }
        public string Lote { get; set; } = string.Empty;
        public string Vencimiento { get; set; } = string.Empty;
    }
}