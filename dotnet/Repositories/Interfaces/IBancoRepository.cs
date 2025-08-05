using System.Text;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IBancoRepository
    {
        Task<IEnumerable<MovimientoBancarioViewModel>> ConsultaMovimientosBancarios(
            string fechaInicio,
            string fechaFin,
            int? estado,
            string? cheque,
            uint? codigoCuenta,
            int? tipoFecha,
            int? guardarCobroTarjeta,
            int? chequeTransferencia
        );

        Task<IEnumerable<CuentaBancariaViewModel>> ConsultarCuentasBancarias(
            int? Estado,
            uint? Moneda
        );

        Task<IEnumerable<MovimientoBancarioViewModel>> GetChequesPendientes(
            string fechaInicio,
            string fechaFin,
            uint? codigoCuenta,
            string? cheque,
            int? estado,
            int? tipoFecha,
            int? checkSaldo,
            int? situacion,
            string? busqueda,
            int? aplicado,
            int? guardarCobroTarjeta,
            int? chequeTransferencia
        );
    }
}