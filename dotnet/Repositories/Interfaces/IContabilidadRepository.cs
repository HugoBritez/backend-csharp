using Api.Models.ViewModels;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Repositories.Interfaces
{
    public interface IContabilidadRepository
    {
        Task<int> BuscarCodigoPlanCuentaCajaDef(uint codigoDefCaja);
        Task<uint> GenerarNroAsiento();
        Task<DatosCajaViewModel> GetDatosCaja(uint operadorCodigo);
        Task<ConfiguracionAsiento> GetConfiguracionAsiento(uint NroTabla);
        decimal QuitarComas(decimal valor);
        decimal RedondearNumero(decimal valor);
        Task<uint> InsertarAsientoContable(AsientoContableDTO asientoContable);
        Task<DetalleAsientoContableDTO> InsertarDetalleAsientoContable(DetalleAsientoContableDTO detalleAsientoContable);
    }
}