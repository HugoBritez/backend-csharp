namespace Api.Models.Dtos
{
    public class CalcularSaldoTotalDTO
{
    public int? Estado { get; set;}
    public uint? Moneda { get; set; }
    public string? FechaInicio { get; set; }
    public string? FechaFin { get; set;}
    public int? Situacion { get; set; }
    public int? CheckSaldo { get; set; }
    public int? ChequeTransferencia { get; set; }
    public string? Cheque { get; set;}
    public uint CodigoCuenta { get; set; }
    public int? TipoFecha { get; set; }
    public string? Busqueda { get; set;}
    public int? Aplicado { get; set; }
}
}