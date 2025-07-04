namespace Api.Models.Dtos
{
    public class DetalleAsientoContableDTO
    {
        public uint Asiento { get; set; }
        public uint Plan { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string Concepto { get; set; } = string.Empty;
    }
}