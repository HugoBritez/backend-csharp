using Api.Models.Dtos;

namespace Api.Services.Interfaces
{
    public interface IContabilidadService
    {
        Task<uint> GuardarAsientoContable(GuardarAsientoContableDTO guardarAsientoContable);

        Task<uint> GuardarCostoAsientoContable(GuardarCostoAsientoContableDTO costoAsiento);
    }
}