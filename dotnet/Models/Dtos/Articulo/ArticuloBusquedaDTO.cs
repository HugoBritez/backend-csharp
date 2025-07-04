// Models/DTOs/ArticuloBusquedaDTO.cs
namespace Api.Models.Dtos.Articulo
{
    public class ArticuloBusquedaDTO
{
    public uint IdLote { get; set; }
    public string? Lote { get; set; }
    public uint IdArticulo { get; set; }
    public string CodInterno { get; set; } = string.Empty;
    public uint ControlVencimiento { get; set; }
    public string? CodigoBarra { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public byte StockNegativo { get; set; }
    public decimal PrecioCosto { get; set; }
    public decimal PrecioVentaGuaranies { get; set; }
    public decimal PrecioCredito { get; set; }
    public decimal PrecioMostrador { get; set; }
    public decimal Precio4 { get; set; }
    public decimal PrecioCostoDolar { get; set; }
    public decimal PrecioVentaDolar { get; set; }
    public decimal PrecioCostoPesos { get; set; }
    public decimal PrecioVentaPesos { get; set; }
    public decimal PrecioCostoReal { get; set; }
    public decimal PrecioVentaReal { get; set; }
    public string? VencimientoLote { get; set; }
    public decimal CantidadLote { get; set; }
    public uint Deposito { get; set; }
    public string? Ubicacion { get; set; }
    public string? SubUbicacion { get; set; }
    public string? Marca { get; set; }
    public string? Subcategoria { get; set; }
    public string? Categoria { get; set; }
    public uint Iva { get; set; }
    public byte VencimientoValidacion { get; set; }
    public string? IvaDescripcion { get; set; }
    public byte EditarNombre { get; set; }
    public string? EstadoVencimiento { get; set; }
    public string? Proveedor { get; set; }
    public string? FechaUltimaVenta { get; set; }
    public decimal PreCompra { get; set; }
}
}
