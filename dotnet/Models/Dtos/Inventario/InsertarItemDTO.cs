namespace Api.Models.Dtos
{
    public class InsertarItemsDTO
    {
        public uint IdArticulo { get; set; }
        public uint IdLote { get; set; }
        public string Lote { get; set; } = string.Empty;
        public DateTime FechaVencimientoItem { get; set; }
        public decimal CantidadInicial { get; set; }
        public decimal CantidadFinal { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
    }
}