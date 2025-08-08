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
            Console.WriteLine($"=== INICIO CONSULTA ===");
            Console.WriteLine($"Parámetros: fechaInicio={fechaInicio}, fechaFin={fechaFin}, estado={estado}, codigoCuenta={codigoCuenta}, aplicado={aplicado}, situacion={situacion}");

            if (!codigoCuenta.HasValue)
                return new ConsultaMovimientosResponse();

            // Inicializar variables como en FoxPro
            var saldoAnterior = 0.0m;
            var aConHaber = 0.0m;
            var conHaber = 0.0m;
            var conDebe = 0.0m;
            var saldoActual = 0.0m;

            var guardarCobroTarjetaResponse = await _configuracionRepository.GetById(17);
            var guardarCobroTarjeta = guardarCobroTarjetaResponse?.Valor ?? "0";

            // Obtener el saldo inicial de la cuenta
            var cuentaSeleccionada = await ConsultarCuentasBancarias(null, null, fechaInicio, fechaFin, null, null, chequeTransferencia);
            var cuenta = cuentaSeleccionada.FirstOrDefault(c => c.Codigo == codigoCuenta.Value);
            saldoAnterior = cuenta?.Saldo ?? 0;
            Console.WriteLine($"Saldo anterior: {saldoAnterior}");

            // Obtener movimientos regulares
            var movimientos = await _bancoRepository.ConsultaMovimientosBancarios(
                fechaInicio, fechaFin, estado, cheque, codigoCuenta, tipoFecha,
                int.Parse(guardarCobroTarjeta), chequeTransferencia);

            Console.WriteLine($"=== MOVIMIENTOS REGULARES ===");
            Console.WriteLine($"Total movimientos regulares: {movimientos.Count()}");
            foreach (var mov in movimientos.Take(3))
            {
                Console.WriteLine($"Mov: {mov.McCodigo} - Fecha: {mov.McFecha} - Haber: {mov.McHaber} - Debito: {mov.McDebito} - Conciliado: {mov.McConciliado}");
            }

            // Obtener cheques pendientes
            var chequesPendientes = await _bancoRepository.GetChequesPendientes(
                fechaInicio, fechaFin, codigoCuenta, cheque, estado, tipoFecha,
                checkSaldo, situacion, busqueda, aplicado, int.Parse(guardarCobroTarjeta), chequeTransferencia);

            Console.WriteLine($"=== CHEQUES PENDIENTES ===");
            Console.WriteLine($"Total cheques pendientes: {chequesPendientes.Count()}");
            
            // Combinar ambos resultados
            var todosLosMovimientos = new List<MovimientoBancarioViewModel>();
            todosLosMovimientos.AddRange(movimientos);

            // Convertir cheques a movimientos
            var chequesComoMovimientos = chequesPendientes.Select(c => new MovimientoBancarioViewModel
            {
                McCodigo = c.McCodigoDetalle,
                McFecha = c.McFecha,
                TmDescripcion = "Cheque Confirmado",
                McCuenta = c.McCuenta,
                CbDescripcion = c.CbDescripcion,
                McHaber = c.McImporte,
                McDebito = 0,
                McSaldo = 0,
                McOrden = c.McOrden,
                McNumero = c.McNumero.ToString(),
                McConciliacion = (uint)c.McConciliado,
                McEstado = c.McEstado,
                McReferencia = c.MCReferencia,
                McCodigoMovimiento = c.McCodigoMovimiento,
                McCodigoDetche = c.McCodigoDetalle,
                McTipoMovimiento = 2,
                McConciliado = c.McConciliado,
                McTransferencia = 0,
                McFechaConciliado = c.McFechaConciliado
            });
            todosLosMovimientos.AddRange(chequesComoMovimientos);

            Console.WriteLine($"=== COMBINACIÓN ===");
            Console.WriteLine($"Total después de combinar: {todosLosMovimientos.Count}");

            // Ordenar por fecha
            var movimientosOrdenados = todosLosMovimientos.OrderBy(m => m.McFecha).ToList();

            Console.WriteLine($"=== MOVIMIENTOS ORDENADOS ===");
            Console.WriteLine($"Total final: {movimientosOrdenados.Count}");
            foreach (var mov in movimientosOrdenados.Take(5))
            {
                Console.WriteLine($"Final: {mov.McCodigo} - Fecha: {mov.McFecha} - Tipo: {mov.TmDescripcion} - Haber: {mov.McHaber} - Debito: {mov.McDebito}");
            }

            // Calcular totales considerando TODOS los movimientos (conciliados y no conciliados)
            var totalCredito = movimientosOrdenados.Sum(m => m.McHaber);
            var totalDebito = movimientosOrdenados.Sum(m => m.McDebito);

            // Mantener las variables originales para compatibilidad con la lógica existente
            if (aplicado == 1)
            {
                Console.WriteLine($"=== CÁLCULO (aplicado=1) ===");
                aConHaber = movimientosOrdenados.Where(m => m.McConciliado == 0).Sum(m => m.McHaber);
                conHaber = movimientosOrdenados.Where(m => m.McConciliado == 1).Sum(m => m.McHaber);
                conDebe = movimientosOrdenados.Where(m => m.McConciliado == 1).Sum(m => m.McDebito);

                Console.WriteLine($"AConHaber: {aConHaber}");
                Console.WriteLine($"ConHaber: {conHaber}");
                Console.WriteLine($"ConDebe: {conDebe}");

                foreach (var movimiento in movimientosOrdenados)
                {
                    movimiento.McSaldo = saldoAnterior + movimiento.McDebito - movimiento.McHaber;
                    saldoAnterior = movimiento.McSaldo;
                }

                saldoActual = saldoAnterior + conHaber - aConHaber - conDebe;
            }
            else
            {
                Console.WriteLine($"=== CÁLCULO (aplicado=0, situacion={situacion}) ===");
                if (situacion == 1)
                {
                    conHaber = movimientosOrdenados.Where(m => m.McConciliado == 1).Sum(m => m.McHaber);
                    conDebe = movimientosOrdenados.Where(m => m.McConciliado == 1).Sum(m => m.McDebito);

                    Console.WriteLine($"ConHaber: {conHaber}");
                    Console.WriteLine($"ConDebe: {conDebe}");

                    foreach (var movimiento in movimientosOrdenados)
                    {
                        movimiento.McSaldo = saldoAnterior + movimiento.McDebito - movimiento.McHaber;
                        saldoAnterior = movimiento.McSaldo;
                    }

                    saldoActual = saldoAnterior + conHaber - aConHaber - conDebe;
                }
                else if (situacion == 2)
                {
                    aConHaber = movimientosOrdenados.Where(m => m.McConciliado == 0).Sum(m => m.McHaber);
                    Console.WriteLine($"AConHaber: {aConHaber}");
                }
                else if (situacion == 3)
                {
                    aConHaber = movimientosOrdenados.Where(m => m.McConciliado == 0).Sum(m => m.McHaber);
                    conHaber = movimientosOrdenados.Where(m => m.McConciliado == 1).Sum(m => m.McHaber);
                    conDebe = movimientosOrdenados.Where(m => m.McConciliado == 1).Sum(m => m.McDebito);

                    Console.WriteLine($"AConHaber: {aConHaber}");
                    Console.WriteLine($"ConHaber: {conHaber}");
                    Console.WriteLine($"ConDebe: {conDebe}");

                    foreach (var movimiento in movimientosOrdenados)
                    {
                        movimiento.McSaldo = saldoAnterior + movimiento.McDebito - movimiento.McHaber;
                        saldoAnterior = movimiento.McSaldo;
                    }

                    saldoActual = saldoAnterior + conHaber - aConHaber - conDebe;
                }
            }

            Console.WriteLine($"=== RESPUESTA FINAL ===");
            Console.WriteLine($"Saldo anterior: {saldoAnterior}");
            Console.WriteLine($"Saldo actual: {saldoActual}");
            Console.WriteLine($"Total movimientos en respuesta: {movimientosOrdenados.Count}");

            var response = new ConsultaMovimientosResponse
            {
                Movimientos = movimientosOrdenados,
                SaldoAnterior = cuenta?.Saldo ?? 0,
                TotalCredito = totalCredito,  // Usar el total de TODOS los movimientos
                TotalDebito = totalDebito,    // Usar el total de TODOS los movimientos
                Total = totalDebito - totalCredito,
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
