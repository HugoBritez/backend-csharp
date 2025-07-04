namespace Api.Models.Dtos
{
    public class ItemConfirmarVerificacionDTO
    {
        public string Lote { get; set; } = string.Empty;
        public int CantidadIngreso { get; set; }
        public int CantidadFactura { get; set; }
        public int IdArticulo { get; set; }
        public DateTime Vencimiento { get; set; }
    }
}