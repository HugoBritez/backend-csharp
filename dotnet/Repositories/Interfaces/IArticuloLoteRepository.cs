using Api.Models.Entities;
using Api.Models.Dtos.ArticuloLote;
namespace Api.Repositories.Interfaces
{
    public interface IArticuloLoteRepository
    {
        Task<IEnumerable<ArticuloLote?>> GetByArticulo(uint articuloId);
        Task<ArticuloLote?> GetById(uint id);

        Task<ArticuloLote> Create(ArticuloLote articuloLote);

        Task<ArticuloLote?> UpdatePartial(uint id, ArticuloLotePatchDTO articuloLotePatchDTO);

        Task<ArticuloLote> Update(ArticuloLote articulo);

        Task<ArticuloLote?> BuscarPorDeposito(uint id_articulo,int control_vencimiento , uint id_deposito, string lote, DateTime vencimiento);
            }
}
