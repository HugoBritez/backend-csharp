namespace Api.Models.ViewModels
{
    public class DetalleCompraConsultaViewModel
    {
        public uint DetalleCompra { get; set; }
        public uint ArticuloId { get; set; }
        public string ArticuloDescripcion { get; set; } = string.Empty;
        public string? ArticuloCodigoBarras { get; set; }
        public int Cantidad { get; set; }
        public int CantidadVerificada { get; set; }
        public string Lote { get; set; } = string.Empty;
        public string Vencimiento { get; set; } = string.Empty;
        public string? Responsable { get; set; }
    }
}