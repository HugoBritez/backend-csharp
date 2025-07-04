namespace Api.Models.ViewModels
{
    public class CompraConsultaViewModel
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
        public int ResponsableUbicacion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}