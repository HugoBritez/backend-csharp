namespace Api.Models.ViewModels
{
    public interface IMovimientoBancarioViewModel
    {
        uint McCodigo { get; set; }
        string? McFecha { get; set; }
        uint McCuenta { get; set; }
        string? CbDescripcion { get; set; }
        int McEstado { get; set; }
        uint McCodigoMovimiento { get; set; }
        int McTipoMovimiento { get; set; }
        int McConciliado { get; set; }
        string? McFechaConciliado { get; set; }
    }
}