namespace Api.Models.ViewModels
{
    public class ChequeViewModel : IMovimientoBancarioViewModel
    {
        public uint McCodigo { get; set; }
        public string? McFecha { get; set; }
        public string? McVencimiento { get; set; }
        public uint McCuenta { get; set; }
        public string? CbDescripcion { get; set; }
        public decimal McImporte { get; set; }
        public string? McOrden { get; set; }
        public decimal McNumero { get; set; }
        public int McEstado { get; set; }
        public int MCReferencia { get; set; }
        public uint McCodigoMovimiento { get; set; }
        public uint McCodigoDetalle { get; set; }
        public int McSituacion { get; set; }
        public int McTipoMovimiento { get; set; }
        public int McConciliado { get; set; }
        public string? McFechaConciliado { get; set; }
        public uint McChequeCobrado { get; set; }
    }
}