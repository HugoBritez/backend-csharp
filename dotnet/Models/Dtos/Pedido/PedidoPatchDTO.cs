namespace Api.Models.Dtos
{
    public class PedidoPatchDto
{
    public DateOnly? Fecha { get; set; }
    public string? NroPedido { get; set; }
    public uint? Cliente { get; set; }
    public uint? Operador { get; set; }
    public uint? Moneda { get; set; }
    public uint? Deposito { get; set; }
    public uint? Sucursal { get; set; }
    public decimal? Descuento { get; set; }
    public string? Observacion { get; set; }
    public int? Estado { get; set; }
    public uint? Vendedor { get; set; }
    public uint? Area { get; set; }
    public uint? TipoEstado { get; set; }
    public uint? Credito { get; set; }
    public int? Imprimir { get; set; }
    public string? Interno { get; set; }
    public string? Latitud { get; set; }
    public string? Longitud { get; set; }
    public int? Tipo { get; set; }
    public decimal? Entrega { get; set; }
    public uint? CantCuotas { get; set; }
    public uint? Consignacion { get; set; }
    public uint? AutorizarContado { get; set; }
    public int? Zona { get; set; }
    public int? Acuerdo { get; set; }
    public int? ImprimirPreparacion { get; set; }
    public int? CantidadCajas { get; set; }
    public int? PreparadoPor { get; set; }
    public int? VerificadoPor { get; set; }
}

}