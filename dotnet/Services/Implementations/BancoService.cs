using Api.Models.Dtos.Banco;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services.Implementations
{
    public class BancoService : IBancoService
    {
        private readonly IBancoRepository _bancoRepository;

        public BancoService(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        public async Task<IEnumerable<CuentaBancariaViewModel>> ConsultarCuentasBancarias(int? estado, uint? moneda, string? fechaInicio, string? fechaFin, int? situacion, int? checkSaldo, int? guardarCobroTarjeta, int? chequeTransferencia)
        {
            var cuentas = await _bancoRepository.ConsultarCuentasBancarias(estado, moneda);
            foreach (var cuenta in cuentas)
            {
                var consultaSaldo = new CalculoSaldoDTO
                {
                    CodigoCuenta = cuenta.Codigo,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    Situacion = situacion,
                    CheckSaldo = checkSaldo,
                    GuardarCobroTarjeta = guardarCobroTarjeta,
                    ChequeTransferencia = chequeTransferencia
                };
                var saldo = await CalcularSaldoTotal(consultaSaldo);
                cuenta.Saldo = saldo;
            }
            return cuentas;
        }

        public async Task<ConsultaMovimientosResponse> ConsultaMovimientosBancarios(
            string fechaInicio, string fechaFin, int? estado, string? cheque,
            uint? codigoCuenta, int? tipoFecha, int? guardarCobroTarjeta, int? chequeTransferencia)
        {
            if (!codigoCuenta.HasValue)
                return new ConsultaMovimientosResponse();

            // Obtener el saldo inicial de la cuenta
            var cuentaSeleccionada = await ConsultarCuentasBancarias(null, null, fechaInicio, fechaFin, null, null, guardarCobroTarjeta, chequeTransferencia);
            var cuenta = cuentaSeleccionada.FirstOrDefault(c => c.Codigo == codigoCuenta.Value);
            var saldoAnterior = cuenta?.Saldo ?? 0;

            var movimientos = await _bancoRepository.ConsultaMovimientosBancarios(
                fechaInicio, fechaFin, estado, cheque, codigoCuenta, tipoFecha, guardarCobroTarjeta, chequeTransferencia);

            foreach (var movimiento in movimientos)
            {
                // Fórmula correcta: Saldo = Saldo anterior + Débitos - Haberes
            movimiento.McSaldo = saldoAnterior + movimiento.McDebito - movimiento.McHaber;
                saldoAnterior = movimiento.McSaldo;
            }

            var response = new ConsultaMovimientosResponse
            {
                Movimientos = movimientos,
                SaldoAnterior = saldoAnterior,
                TotalCredito = movimientos.Sum(m => m.McHaber),
                TotalDebito = movimientos.Sum(m => m.McDebito),
                Total = movimientos.Sum(m => m.McDebito - m.McHaber),
                SaldoActual = saldoAnterior + movimientos.Sum(m => m.McDebito - m.McHaber)
            };
            return response; 
        }

        public async Task<IEnumerable<ChequeViewModel>> GetChequesPendientes(
            string fechaInicio, string fechaFin, uint? codigoCuenta, string? cheque,
            int? estado, int? tipoFecha, int? checkSaldo, int? situacion, string? busqueda,
            int? aplicado, int? guardarCobroTarjeta, int? chequeTransferencia)
        {
            return await _bancoRepository.GetChequesPendientes(
                fechaInicio, fechaFin, codigoCuenta, cheque, estado, tipoFecha, checkSaldo,
                situacion, busqueda, aplicado, guardarCobroTarjeta, chequeTransferencia);
        }

        public async Task<decimal> CalcularSaldoTotal(CalculoSaldoDTO dto)
        {
            // Si no hay fechas, retornar 0 como en FoxPro
            if (string.IsNullOrEmpty(dto.FechaInicio) || string.IsNullOrEmpty(dto.FechaFin))
                return 0;

            var debe = await _bancoRepository.CalcularDebeCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, dto.GuardarCobroTarjeta, dto.ChequeTransferencia);

            var haber = await _bancoRepository.CalcularHaberCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, dto.GuardarCobroTarjeta, dto.ChequeTransferencia);

            var chequesPropios = await _bancoRepository.CalcularChequeCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, dto.GuardarCobroTarjeta, dto.ChequeTransferencia);

            var chequesRecibidos = await _bancoRepository.CalcularChequeRecibidoCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, dto.GuardarCobroTarjeta, dto.ChequeTransferencia);

            // Cálculo exacto como en FoxPro
            var saldoAnterior = debe - haber - chequesPropios - chequesRecibidos;

            return saldoAnterior;
        }
    }
}
