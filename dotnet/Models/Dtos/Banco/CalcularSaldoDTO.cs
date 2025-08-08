namespace Api.Models.Dtos.Banco
{
    public class CalculoSaldoDTO
    {
        public uint CodigoCuenta { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public int? Situacion { get; set; }
        public int? CheckSaldo { get; set; }
        public int? ChequeTransferencia { get; set; }
        public int? GuardarCobroTarjeta { get; set; }
    }
}