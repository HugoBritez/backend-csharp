namespace Api.Models.Dtos
{
    public class GuardarAsientoContableDTO
    {
        public bool Automatico { get; set; }
        public uint TipoVenta { get; set; }
        public uint Moneda { get; set; }
        public uint Sucursal { get; set; }
        public string Factura { get; set; } = string.Empty;
        public uint Operador { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TotalAPagar { get; set; }
        public uint NumeroAsiento { get; set; }
        public decimal Cotizacion { get; set; }
        public decimal TotalExentas { get; set; }
        public decimal TotalCinco { get; set; }
        public decimal TotalDiez { get; set; }
        public uint ImprimirLegal { get; set; }
        public uint? CajaDefinicion { get; set; }
        public int? Configuracion { get; set; }
        public uint Referencia { get; set; }
        public uint? Servicio { get; set; }
    }
}