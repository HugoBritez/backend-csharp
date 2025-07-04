namespace Api.Models.Dtos
{
    public class AsientoContableDTO
    {
        public uint Sucursal { get; set; }
        public uint Moneda { get; set; }
        public uint Operador { get; set; }
        public string Documento { get; set; } = string.Empty;
        public uint Numero { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaAsiento { get; set; }
        public decimal TotalDebe { get; set; }
        public decimal TotalHaber { get; set; }
        public decimal Cotizacion { get; set; }
        public uint Referencia { get; set; }
        public uint Origen { get; set; }
        
        // Campos adicionales requeridos por la base de datos
        public DateTime FechaCierre { get; set; } = new DateTime(1, 1, 1);
        public DateTime FechaApertura { get; set; } = new DateTime(1, 1, 1);
        public decimal CotizacionC { get; set; } = 0;
        public uint TipoAsiento { get; set; } = 0;
        public uint CierreAsiento { get; set; } = 0;
        public uint Estado { get; set; } = 1;
    }
}