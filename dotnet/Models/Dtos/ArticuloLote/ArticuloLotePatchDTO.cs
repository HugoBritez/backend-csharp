namespace Api.Models.Dtos.ArticuloLote
{
    public class ArticuloLotePatchDTO
    {
        public uint? AlArticulo { get; set; }
        public uint? AlDeposito { get; set; }
        public string? AlLote { get; set; }
        public decimal? AlCantidad { get; set; }
        public DateTime? AlVencimiento { get; set; }
        public uint? AlPreCompra { get; set; }
        public uint? AlOrigen { get; set; }
        public string? ALSerie { get; set; }
        public string? AlCodBarra { get; set; }
        public string? AlNroTalle { get; set; }
        public uint? AlColor { get; set; }
        public uint? AlTalle { get; set; }
        public string? AlRegistro { get; set; }
    }
}