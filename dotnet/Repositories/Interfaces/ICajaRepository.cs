using Api.Models.Dtos;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces{
    public interface ICajaRepository
    {
        Task<IEnumerable<CajaViewModel>> VerificarCajaAbierta(uint operador, uint moneda);
    }
}
