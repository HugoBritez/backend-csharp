using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Mysqlx.Datatypes;

namespace Api.Repositories.Interfaces{
    public interface IFinancieroRepository
    {
        Task<IEnumerable<TimbradoResult>> ObtenerDatosFacturacion(uint usuario, uint sucursal);
    }
}
