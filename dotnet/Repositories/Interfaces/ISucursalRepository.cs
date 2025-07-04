using Api.Models.Dtos.Sucursal;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ISucursalRepository
    {
        Task<IEnumerable<SucursalDTO>> GetSucursales(
            uint? operador = null,
            uint? matriz = null
        );
    }
}
