using Api.Models.Dtos;
using Api.Models.Dtos.Banco;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IBancoService
    {
        Task<IEnumerable<CuentaBancariaViewModel>> ConsultarCuentasBancarias(
            int? estado,
            uint? moneda,
            string? fechaInicio,
            string? fechaFin,
            int? situacion,
            int? checkSaldo,
            int? chequeTransferencia
            );
        Task<ConsultaMovimientosResponse> ConsultaMovimientosBancarios(
            string fechaInicio,
            string fechaFin,
            int? estado,
            string? cheque,
            uint? codigoCuenta,
            int? tipoFecha,
            int? chequeTransferencia,
            int? checkSaldo,
            int? situacion,
            string? busqueda,
            int? aplicado);
        Task<IEnumerable<ChequeViewModel>> GetChequesPendientes(
            string fechaInicio, string fechaFin, uint? codigoCuenta, string? cheque,
            int? estado, int? tipoFecha, int? checkSaldo, int? situacion, string? busqueda,
            int? aplicado,  int? chequeTransferencia);
        Task<decimal> CalcularSaldoTotal(CalcularSaldoTotalDTO dto); 


        Task<IEnumerable<CuentaBancariaViewModel>> GetCuentasBancarias(
            string fechaInicio,
            string fechaFin,
            int? estado,
            string? cheque,
            int? tipoFecha,
            int? chequeTransferencia,
            int? checkSaldo,
            int? situacion,
            string? busqueda,
            int? aplicado,
            uint? moneda
        );     
    }
}



