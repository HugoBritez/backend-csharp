using Api.Models.Dtos.Banco;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services.Implementations
{
    public class BancoService : IBancoService
    {
        private readonly IBancoRepository _bancoRepository;

        private readonly IConfiguracionRepository _configuracionRepository;

        public BancoService(IBancoRepository bancoRepository, IConfiguracionRepository configuracionRepository)
        {
            _bancoRepository = bancoRepository;
            _configuracionRepository = configuracionRepository;
        }

        public async Task<IEnumerable<CuentaBancariaViewModel>> ConsultarCuentasBancarias(
            int? estado,
            uint? moneda,
            string? fechaInicio,
            string? fechaFin,
            int? situacion,
            int? checkSaldo,
            int? chequeTransferencia
            )
        {
            var cuentas = await _bancoRepository.ConsultarCuentasBancarias(estado, moneda);
            foreach (var cuenta in cuentas)
            {
                var guardarCobroTarjetaResponse = await _configuracionRepository.GetById(17);
                var guardarCobroTarjeta = guardarCobroTarjetaResponse?.Valor ?? "0";

                var consultaSaldo = new CalculoSaldoDTO
                {
                    CodigoCuenta = cuenta.Codigo,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    Situacion = situacion,
                    CheckSaldo = checkSaldo,
                    ChequeTransferencia = chequeTransferencia,
                    GuardarCobroTarjeta = int.Parse(guardarCobroTarjeta)
                };
                var saldo = await CalcularSaldoTotal(consultaSaldo);
                cuenta.Saldo = saldo;
            }
            return cuentas;
        }

        private static MovimientoBancarioViewModel ConvertirChequeAMovimiento(ChequeViewModel cheque)
        {
            return new MovimientoBancarioViewModel
            {
                McCodigo = cheque.McCodigoDetalle,
                McFecha = cheque.McFecha,
                TmDescripcion = "Cheque Confirmado",
                McCuenta = cheque.McCuenta,
                CbDescripcion = cheque.CbDescripcion,
                McHaber = cheque.McImporte,
                McDebito = 0,
                McSaldo = 0,
                McOrden = cheque.McOrden,
                McNumero = cheque.McNumero.ToString(),
                McConciliacion = (uint)cheque.McConciliado,
                McEstado = cheque.McEstado,
                McReferencia = cheque.MCReferencia,
                McCodigoMovimiento = cheque.McCodigoMovimiento,
                McCodigoDetche = cheque.McCodigoDetalle,
                McTipoMovimiento = 2,
                McConciliado = cheque.McConciliado,
                McTransferencia = 0,
                McFechaConciliado = cheque.McFechaConciliado
            };
        }

        public async Task<ConsultaMovimientosResponse> ConsultaMovimientosBancarios(
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
            int? aplicado)
        {

            if (!codigoCuenta.HasValue)
                return new ConsultaMovimientosResponse();

            // Inicializar variables como en FoxPro
            var saldoAnterior = 0.0m;
            var aConHaber = 0.0m;
            var conHaber = 0.0m;
            var conDebe = 0.0m;
            var saldoActual = 0.0m;
            var movimientos = new List<MovimientoBancarioViewModel>();
            var saldoCuenta = 0.0m;

            var guardarCobroTarjetaResponse = await _configuracionRepository.GetById(17);
            var guardarCobroTarjeta = guardarCobroTarjetaResponse?.Valor ?? "0";

            // Obtener el saldo inicial de la cuenta
            var cuentaSeleccionada = await ConsultarCuentasBancarias(null, null, fechaInicio, fechaFin, null, null, chequeTransferencia);
            var cuenta = cuentaSeleccionada.FirstOrDefault(c => c.Codigo == codigoCuenta.Value);
            saldoAnterior = cuenta?.Saldo ?? 0;
            saldoCuenta = saldoAnterior;

            // Obtener movimientos regulares
            if(aplicado == 1)
            {
                var chequesPendientes = await GetChequesPendientes(
                    fechaInicio,
                    fechaFin,
                    codigoCuenta,
                    cheque,
                    estado,
                    tipoFecha,
                    checkSaldo,
                    situacion,
                    busqueda,
                    aplicado,
                    chequeTransferencia
                    );

                var chequesPendientesMovimientos = chequesPendientes.Select(ConvertirChequeAMovimiento).ToList();

                // Calcular saldo acumulado para cada movimiento
                var saldoAcumulado = saldoAnterior;
                foreach(var movimiento in chequesPendientesMovimientos)
                {
                    saldoAcumulado += movimiento.McDebito - movimiento.McHaber;
                    movimiento.McSaldo = saldoAcumulado;
                }

                movimientos = chequesPendientesMovimientos;
                conHaber = chequesPendientesMovimientos.Sum(m => m.McHaber);
                conDebe = chequesPendientesMovimientos.Sum(m => m.McDebito);
                saldoActual = saldoAcumulado;
                aConHaber = conHaber;
            }
            else
            {
                if(situacion == 1)
                {
                    if(!string.IsNullOrEmpty(cheque))
                    {
                        var chequesPendientes = await GetChequesPendientes(
                            fechaInicio,
                            fechaFin,
                            codigoCuenta,
                            cheque,
                            estado,
                            tipoFecha,
                            checkSaldo,
                            situacion,
                            busqueda,
                            aplicado,
                            chequeTransferencia
                            );
                        var chequesPendientesMovimientos = chequesPendientes.Select(c => ConvertirChequeAMovimiento(c)).ToList();

                        // Calcular saldo acumulado para cada movimiento
                        var saldoAcumulado = saldoAnterior;
                        foreach(var movimiento in chequesPendientesMovimientos)
                        {
                            saldoAcumulado += movimiento.McDebito - movimiento.McHaber;
                            movimiento.McSaldo = saldoAcumulado;
                        }

                        movimientos = chequesPendientesMovimientos;
                        conHaber = chequesPendientesMovimientos.Sum(m => m.McHaber);
                        conDebe = chequesPendientesMovimientos.Sum(m => m.McDebito);
                        saldoActual = saldoAcumulado;
                        aConHaber = conHaber;
                    }
                    else
                    {
                        var movimientosBancarios = await _bancoRepository.ConsultaMovimientosBancarios(
                            fechaInicio,
                            fechaFin,
                            estado,
                            cheque,
                            codigoCuenta,
                            tipoFecha, 
                            int.Parse(guardarCobroTarjeta),
                            chequeTransferencia
                            );

                        var chequesPendientes = await GetChequesPendientes(
                            fechaInicio,
                            fechaFin,
                            codigoCuenta,
                            cheque,
                            estado,
                            tipoFecha,
                            checkSaldo,
                            situacion,
                            busqueda,
                            aplicado,
                            chequeTransferencia
                            );

                        var chequesPendientesMovimientos = chequesPendientes.Select(c => ConvertirChequeAMovimiento(c)).ToList();

                        // Combinar y ordenar por fecha
                        var todosLosMovimientos = new List<MovimientoBancarioViewModel>();
                        todosLosMovimientos.AddRange(movimientosBancarios);
                        todosLosMovimientos.AddRange(chequesPendientesMovimientos);
                        var movimientosOrdenados = todosLosMovimientos.OrderBy(m => m.McFecha).ToList();

                        // Calcular saldo acumulado para cada movimiento
                        var saldoAcumulado = saldoAnterior;
                        foreach(var movimiento in movimientosOrdenados)
                        {
                            saldoAcumulado += movimiento.McDebito - movimiento.McHaber;
                            movimiento.McSaldo = saldoAcumulado;
                        }

                        movimientos = movimientosOrdenados;
                        conHaber = movimientosBancarios.Sum(m => m.McHaber);
                        conDebe = movimientosBancarios.Sum(m => m.McDebito);
                        saldoActual = saldoAcumulado;
                        aConHaber = conHaber;
                    }
                }
                else if (situacion == 2)
                {
                    var chequesPendientes = await GetChequesPendientes(
                        fechaInicio,
                        fechaFin,
                        codigoCuenta,
                        cheque,
                        estado,
                        tipoFecha,
                        checkSaldo,
                        situacion,
                        busqueda,
                        aplicado,
                        chequeTransferencia
                        );
                    var chequesPendientesMovimientos = chequesPendientes.Select(c => ConvertirChequeAMovimiento(c)).ToList();

                    // Calcular saldo acumulado para cada movimiento
                    var saldoAcumulado = saldoAnterior;
                    foreach(var movimiento in chequesPendientesMovimientos)
                    {
                        saldoAcumulado += movimiento.McDebito - movimiento.McHaber;
                        movimiento.McSaldo = saldoAcumulado;
                    }

                    movimientos = chequesPendientesMovimientos;
                    conHaber = chequesPendientesMovimientos.Sum(m => m.McHaber);
                    conDebe = chequesPendientesMovimientos.Sum(m => m.McDebito);
                    saldoActual = saldoAcumulado;
                    aConHaber = conHaber;
                }
                else if (situacion == 3)
                {
                    if(!string.IsNullOrEmpty(cheque))
                    {
                        var chequesPendientes = await GetChequesPendientes(
                            fechaInicio,
                            fechaFin,
                            codigoCuenta,
                            cheque,
                            estado,
                            tipoFecha,
                            checkSaldo,
                            situacion,
                            busqueda,
                            aplicado,
                            chequeTransferencia);
                        var chequesPendientesMovimientos = chequesPendientes.Select(c => ConvertirChequeAMovimiento(c)).ToList();

                        // Calcular saldo acumulado para cada movimiento
                        var saldoAcumulado = saldoAnterior;
                        foreach(var movimiento in chequesPendientesMovimientos)
                        {
                            saldoAcumulado += movimiento.McDebito - movimiento.McHaber;
                            movimiento.McSaldo = saldoAcumulado;
                        }

                        movimientos = chequesPendientesMovimientos;
                        conHaber = chequesPendientesMovimientos.Sum(m => m.McHaber);
                        conDebe = chequesPendientesMovimientos.Sum(m => m.McDebito);
                        saldoActual = saldoAcumulado;
                        aConHaber = conHaber;
                    }
                    else
                    {
                        var movimientosBancarios = await _bancoRepository.ConsultaMovimientosBancarios(fechaInicio, fechaFin, estado, cheque, codigoCuenta, tipoFecha, int.Parse(guardarCobroTarjeta), chequeTransferencia);
                        var chequesPendientes = await GetChequesPendientes(fechaInicio, fechaFin, codigoCuenta, cheque, estado, tipoFecha, checkSaldo, situacion, busqueda, aplicado, chequeTransferencia);
                        var chequesPendientesMovimientos = chequesPendientes.Select(c => ConvertirChequeAMovimiento(c)).ToList();

                        // Combinar y ordenar por fecha
                        var todosLosMovimientos = new List<MovimientoBancarioViewModel>();
                        todosLosMovimientos.AddRange(movimientosBancarios);
                        todosLosMovimientos.AddRange(chequesPendientesMovimientos);
                        var movimientosOrdenados = todosLosMovimientos.OrderBy(m => m.McFecha).ToList();

                        // Calcular saldo acumulado para cada movimiento
                        var saldoAcumulado = saldoAnterior;
                        foreach(var movimiento in movimientosOrdenados)
                        {
                            saldoAcumulado += movimiento.McDebito - movimiento.McHaber;
                            movimiento.McSaldo = saldoAcumulado;
                        }

                        movimientos = movimientosOrdenados;
                        conHaber = movimientosBancarios.Sum(m => m.McHaber);
                        conDebe = movimientosBancarios.Sum(m => m.McDebito);
                        saldoActual = saldoAcumulado;
                        aConHaber = conHaber;   
                    }
                }
            }


            var response = new ConsultaMovimientosResponse
            {
                Movimientos = movimientos,
                SaldoAnterior = saldoAnterior,
                TotalCredito = conHaber,
                TotalDebito = conDebe,
                Total = conDebe - conHaber,
                SaldoActual = saldoActual,
                AConHaber = aConHaber
            };
            
            return response; 
        }

        public async Task<IEnumerable<ChequeViewModel>> GetChequesPendientes(
            string fechaInicio, string fechaFin, uint? codigoCuenta, string? cheque,
            int? estado, int? tipoFecha, int? checkSaldo, int? situacion, string? busqueda,
            int? aplicado,  int? chequeTransferencia)
        {
            var guardarCobroTarjetaResponse = await _configuracionRepository.GetById(17);
            var guardarCobroTarjeta = guardarCobroTarjetaResponse?.Valor ?? "0";

            return await _bancoRepository.GetChequesPendientes(
                fechaInicio, fechaFin, codigoCuenta, cheque, estado, tipoFecha, checkSaldo,
                situacion, busqueda, aplicado, int.Parse(guardarCobroTarjeta), chequeTransferencia);
        }

        public async Task<decimal> CalcularSaldoTotal(CalculoSaldoDTO dto)
        {
            var guardarCobroTarjetaResponse = await _configuracionRepository.GetById(17);
            var guardarCobroTarjeta = guardarCobroTarjetaResponse?.Valor ?? "0";

            var debe = await _bancoRepository.CalcularDebeCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, int.Parse(guardarCobroTarjeta), dto.ChequeTransferencia);

            var haber = await _bancoRepository.CalcularHaberCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, int.Parse(guardarCobroTarjeta), dto.ChequeTransferencia);

            var chequesPropios = await _bancoRepository.CalcularChequeCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, int.Parse(guardarCobroTarjeta), dto.ChequeTransferencia);

            var chequesRecibidos = await _bancoRepository.CalcularChequeRecibidoCuentabancaria(
                dto.CodigoCuenta, dto.FechaInicio, dto.FechaFin, dto.Situacion, 
                dto.CheckSaldo, int.Parse(guardarCobroTarjeta), dto.ChequeTransferencia);

            // Cálculo exacto como en FoxPro
            var saldoAnterior = debe - haber - chequesPropios - chequesRecibidos;

            return saldoAnterior;
        }
    }
}
