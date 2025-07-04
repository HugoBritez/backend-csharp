namespace Api.Models.Dtos
{
    public class GuardarCostoAsientoContableDTO
    {
        public bool Automatico { get; set; }
        public uint Moneda { get; set; }
        public uint Sucursal { get; set; }
        public string Factura { get; set; } = string.Empty;
        public uint Operador { get; set; }
        public DateTime Fecha { get; set; }
        public decimal CostoTotalCinco { get; set; }
        public decimal CostoTotalDiez { get; set; }
        public decimal CostoTotalExentas { get; set; }

        public decimal Cotizacion { get; set; }
        public uint MonedaDolar { get; set; }
        public uint ImprimirLegal { get; set; }
        public uint Referencia { get; set; }
    }
}