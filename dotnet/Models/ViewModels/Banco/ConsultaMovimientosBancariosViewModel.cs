namespace Api.Models.ViewModels
{
    public class MovimientoBancarioViewModel : IMovimientoBancarioViewModel
    {
        public uint McCodigo { get; set; }
        public string? McFecha { get; set; }
        public string? TmDescripcion { get; set; }
        public uint McCuenta { get; set; }
        public string? CbDescripcion { get; set; }
        public decimal McHaber { get; set; }
        public decimal McDebito { get; set; }
        public decimal McSaldo { get; set; }
        public string? McOrden { get; set; }
        public string? McNumero { get; set; }
        public uint McConciliacion { get; set; }
        public int McEstado { get; set; }
        public int McReferencia { get; set; }
        public uint McCodigoMovimiento { get; set; }
        public uint McCodigoDetche { get; set; }
        public int McTipoMovimiento { get; set; }
        public int McConciliado { get; set; }
        public uint McTransferencia { get; set; }
        public string? McFechaConciliado { get; set; }
    }

}
