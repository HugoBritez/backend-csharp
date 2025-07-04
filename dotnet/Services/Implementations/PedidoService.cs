using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Audit.Services;
using Api.Models.ViewModels;
using Api.Auth.Services;
using Api.Auth.Models;
namespace Api.Services.Implementations
{
    public class PedidoService : IPedidosService
    {
        private readonly IPedidosRepository _pedidoRepository;
        private readonly IDetallePedidoRepository _detallePedidoRepository;
        private readonly IDetallePedidoFaltanteRepository _detallePedidoFaltanteRepository;
        private readonly IAuditoriaService _auditoriaService;
        private readonly IArticuloLoteRepository _articuloLoteRepository;
        private readonly IAreaSecuenciaRepository _areaSecuenciaRepository;
        private readonly IPedidoEstadoRepository _pedidoEstadoRepository;

        private readonly IAuthService _authService;

        public PedidoService(
            IPedidosRepository pedidosRepository,
            IDetallePedidoRepository detallePedidoRepository,
            IDetallePedidoFaltanteRepository detallePedidoFaltanteRepository,
            IAuditoriaService auditoriaService,
            IArticuloLoteRepository articuloLoteRepository,
            IAreaSecuenciaRepository areaSecuenciaRepository,
            IPedidoEstadoRepository pedidoEstadoRepository,
            IAuthService authService
        )
        {
            _pedidoRepository = pedidosRepository;
            _detallePedidoRepository = detallePedidoRepository;
            _detallePedidoFaltanteRepository = detallePedidoFaltanteRepository;
            _auditoriaService = auditoriaService;
            _articuloLoteRepository = articuloLoteRepository;
            _areaSecuenciaRepository = areaSecuenciaRepository;
            _pedidoEstadoRepository = pedidoEstadoRepository;
            _authService = authService;
        }

        public async Task<Pedido> CrearPedido(Pedido pedido, IEnumerable<DetallePedido> detallePedido)
        {
            var pedidoCreado = await _pedidoRepository.CrearPedido(pedido);

            foreach (DetallePedido detalle in detallePedido)
            {
                DetallePedido detalleCreado = await _detallePedidoRepository.Crear(detalle);

                // Verificar la cantidad disponible en el lote
                var loteDetalle = detalle.CodigoLote;
                ArticuloLote? loteExistente = await _articuloLoteRepository.GetById(loteDetalle);

                if (loteExistente != null)
                {
                    // Caso 1: La cantidad solicitada es menor que la disponible en el lote
                    if (detalleCreado.Cantidad < loteExistente.AlCantidad)
                    {
                        // No hay faltantes, hay suficiente stock
                        Console.WriteLine($"Stock suficiente. Disponible: {loteExistente.AlCantidad}, Solicitado: {detalleCreado.Cantidad}");
                    }
                    // Caso 2: La cantidad solicitada es igual a la disponible en el lote
                    else if (detalleCreado.Cantidad == loteExistente.AlCantidad)
                    {
                        // No hay faltantes, pero se agota el stock completamente
                        Console.WriteLine($"Stock exacto. Disponible: {loteExistente.AlCantidad}, Solicitado: {detalleCreado.Cantidad}");
                    }
                    // Caso 3: La cantidad solicitada es mayor que la disponible en el lote
                    else
                    {
                        // Hay faltantes, registrar la cantidad que excede lo disponible
                        int cantidadFaltante = (int)(detalleCreado.Cantidad - loteExistente.AlCantidad);

                        var detalleFaltante = new DetallePedidoFaltante
                        {
                            Codigo = 0,
                            DetallePedido = detalleCreado.Codigo,
                            Cantidad = cantidadFaltante,
                            Situacion = 0,
                            Observacion = $"Stock insuficiente. Disponible: {loteExistente.AlCantidad}, Solicitado: {detalleCreado.Cantidad}"
                        };

                        await _detallePedidoFaltanteRepository.Crear(detalleFaltante);
                    }
                }
                else
                {
                    // Este caso no debería ocurrir según los requisitos, pero lo dejamos como protección
                    Console.WriteLine("Error: Lote no encontrado, esto no debería ocurrir según los requisitos");
                }
            }

            return pedidoCreado;
        }

        public async Task<string> AnularPedido(uint codigo, string motivo)
        {
            var pedidoAAnular = await _pedidoRepository.GetById(codigo);

            if (pedidoAAnular == null)
            {
                return "Pedido no encontrado";
            }

            if (pedidoAAnular.Estado == 0)
            {
                return "El pedido ya ha sido anulado";
            }

            pedidoAAnular.Estado = 0;

            pedidoAAnular.Observacion = "Pedido anulado";
            await _pedidoRepository.SaveChangesAsync();
            return "Pedido anulado satisfactoriamente";
        }

        public async Task<IEnumerable<PedidoViewModel>> GetPedidos(
        string? fechaDesde,
        string? fechaHasta,
        string? nroPedido,
        int? articulo,
        string? clientes,
        string? vendedores,
        string? sucursales,
        string? estado,
        int? moneda,
        string? factura
        )
        {
            var pedidos = await _pedidoRepository.GetPedidos(
                fechaDesde, fechaHasta, nroPedido, articulo,
                clientes, vendedores, sucursales, estado, moneda, factura
            );

            return pedidos;
        }

        public async Task<ResponseViewModel<Pedido>> AutorizarPedido(
            uint idPedido,
            string Usuario,
            string Password
        )
        {
            LoginResponse loginResponse = await _authService.Login(Usuario, Password);
            if (loginResponse == null)
            {
                return new ResponseViewModel<Pedido>
                {
                    Success = false,
                    Message = "Usuario o contraseña incorrectos."
                };
            }
            if (loginResponse.Usuario[0].OpAutorizar != 1)
            {
                return new ResponseViewModel<Pedido>
                {
                    Success = false,
                    Message = "Usuario sin permisos para autorizar un pedido."
                };
            }

            var pedidoAAutorizar = await _pedidoRepository.GetById(idPedido);
            var areaPedidoActual = pedidoAAutorizar.Area;

            if (areaPedidoActual == 3)
            {
                return new ResponseViewModel<Pedido>
                {
                    Success = false,
                    Message = "Pedido en tesoreria, autorice desde el modulo ventas."
                };
            }

            if (areaPedidoActual == 2)
            {
                return new ResponseViewModel<Pedido>
                {
                    Success = false,
                    Message = "Pedido ya se encuentra en el area 'Ventas'"
                };
            }
            var siguienteArea = await _areaSecuenciaRepository.GetSiguienteArea(areaPedidoActual);

            pedidoAAutorizar.Area = siguienteArea;
            await _pedidoRepository.SaveChangesAsync();
            var pedidoEstadoNuevo = new PedidosEstados
            {
                Codigo = 0,
                Pedido = pedidoAAutorizar.Codigo,
                Area = siguienteArea,
                Operador = loginResponse.Usuario[0].OpNombre,
                Fecha = DateTime.Now
            };
            await _pedidoEstadoRepository.Crear(pedidoEstadoNuevo);

            return new ResponseViewModel<Pedido>
            {
                Success = true,
                Message = "Pedido autorizado satisfactoriamente"
            };
        }
    }
}