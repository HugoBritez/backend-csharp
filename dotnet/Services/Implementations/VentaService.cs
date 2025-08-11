using Api.Models.Dtos;
using Api.Models.Dtos.ArticuloLote;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Services.Mappers;
using Api.Audit.Services;
using Api.Models.ViewModels;
namespace Api.Services.Implementations
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IDetalleVentaRepository _detalleVentaRepository;
        private readonly IDetalleBonificacionRepository _detalleBonificacionRepository;
        private readonly IDetalleArticulosEditadoRepository _detalleArticulosEditadoRepository;
        private readonly IDetalleVentaVencimientoRepository _detalleVentaVencimientoRepository;
        private readonly IArticuloLoteRepository _articuloLoteRepository;
        private readonly IContabilidadService _contabilidadService;
        private readonly IAuditoriaService _auditoriaService;
        private readonly ICotizacionRepository _cotizacionRepository;
        private readonly IMetasService _metasService;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProveedoresRepository _proveedoresRepository;
        public VentaService(IVentaRepository ventaRepository,
            IDetalleVentaRepository detalleVentaRepository,
            IDetalleBonificacionRepository detalleBonificacionRepository,
            IDetalleArticulosEditadoRepository detalleArticulosEditadoRepository,
            IDetalleVentaVencimientoRepository detalleVentaVencimientoRepository,
            IArticuloLoteRepository articuloLoteRepository,
            IContabilidadService contabilidadService,
            IAuditoriaService auditoriaService,
            ICotizacionRepository cotizacionRepository,
            IMetasService metasService,
            IClienteRepository clienteRepository,
            IProveedoresRepository proveedoresRepository
        )
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _detalleBonificacionRepository = detalleBonificacionRepository;
            _detalleArticulosEditadoRepository = detalleArticulosEditadoRepository;
            _detalleVentaVencimientoRepository = detalleVentaVencimientoRepository;
            _articuloLoteRepository = articuloLoteRepository;
            _contabilidadService = contabilidadService;
            _auditoriaService = auditoriaService;
            _cotizacionRepository = cotizacionRepository;
            _metasService = metasService;
            _clienteRepository = clienteRepository;
            _proveedoresRepository = proveedoresRepository;
        }
        public async Task<Venta> CrearVenta(VentaDTO venta, IEnumerable<DetalleVentaDTO> detalleVentaDTOs)
        {
            var ventaCreada = await _ventaRepository.CrearVenta(venta);
            decimal totalExentas = 0;
            decimal totalCinco = 0;
            decimal totalDiez = 0;
            decimal costoTotalExentas = 0;
            decimal costoTotalCinco = 0;
            decimal costoTotalDiez = 0;

            foreach (var detalleDTO in detalleVentaDTOs)
            {
                var detalleVenta = detalleDTO.toDetalleVenta();

                if (detalleVenta != null)
                {
                    totalExentas += detalleVenta.Exentas;
                    totalCinco += detalleVenta.Cinco;
                    totalDiez += detalleVenta.Diez;

                    if (detalleVenta.Exentas > 0)
                    {
                        costoTotalExentas += detalleVenta.Costo;
                    }
                    else if (detalleVenta.Cinco > 0)
                    {
                        costoTotalCinco += detalleVenta.Costo;
                    }
                    else if (detalleVenta.Diez > 0)
                    {
                        costoTotalDiez += detalleVenta.Costo;
                    }

                    detalleVenta.Venta = ventaCreada.Codigo;
                    var detalleVentaCreado = await _detalleVentaRepository.CrearDetalleVenta(detalleVenta);
                    var idDetalleVenta = detalleVentaCreado.Codigo;

                    Console.WriteLine("LoteId del detalleDTO: " + detalleDTO.LoteId);

                    // Lote
                    if (detalleDTO.LoteId != 0)
                    {
                        Console.WriteLine("LoteId del detalleDTO no es cero, procesando lote..." + detalleDTO.LoteId + " para DetalleVenta " + idDetalleVenta); ;
                        var detalleVencimiento = detalleDTO.ToDetalleVencimiento((int)idDetalleVenta);
                        Console.WriteLine("DetalleVencimiento creado: " + (detalleVencimiento != null));
                        if (detalleVencimiento != null)
                        {
                            try
                            {
                                await _detalleVentaVencimientoRepository.CrearDetalleVencimiento(detalleVencimiento);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Error al crear vencimiento para DetalleVenta {idDetalleVenta}, Lote {detalleDTO.LoteId}: {ex.Message}");
                            }
                        }

                        var articuloLoteActual = await _articuloLoteRepository.GetById((uint)detalleDTO.LoteId);
                        if (articuloLoteActual != null)
                        {
                            var articuloLotePatch = new ArticuloLotePatchDTO
                            {
                                AlCantidad = articuloLoteActual.AlCantidad - detalleDTO.DeveCantidad,
                            };

                            await _articuloLoteRepository.UpdatePartial(articuloLoteActual.AlCodigo, articuloLotePatch);
                        }
                    }

                    // Bonificación
                    if (detalleDTO.DeveBonificacion != 0)
                    {
                        var detalleBonificacion = detalleDTO.ToBonificacion((int)idDetalleVenta);
                        if (detalleBonificacion != null)
                        {
                            await _detalleBonificacionRepository.CrearDetalleBonificacion(detalleBonificacion);
                        }
                    }

                    // Artículo editado
                    if (detalleDTO.ArticuloEditado)
                    {
                        var detalleArticuloEditado = detalleDTO.ToDetalleArticulosEditado((int)idDetalleVenta);
                        if (detalleArticuloEditado != null)
                        {
                            await _detalleArticulosEditadoRepository.CrearDetalleArticulosEditado(detalleArticuloEditado);
                        }
                    }
                }
            }

            var imprimirLegal = !string.IsNullOrWhiteSpace(venta.Factura) ? 1u : 0u;
            var cotizacionDolar = await _cotizacionRepository.GetCotizacionDolarHoy();

            var asientoContable = new GuardarAsientoContableDTO
            {
                Automatico = true,
                TipoVenta = ventaCreada.Credito,
                Moneda = ventaCreada.Moneda,
                Sucursal = ventaCreada.Sucursal,
                Factura = ventaCreada.Factura,
                Operador = ventaCreada.Vendedor,
                Fecha = ventaCreada.Fecha,
                TotalAPagar = ventaCreada.Total,
                NumeroAsiento = venta.Codigo,
                Cotizacion = cotizacionDolar != null ? cotizacionDolar.Monto : 7300,
                TotalExentas = totalExentas,
                TotalCinco = totalCinco,
                TotalDiez = totalDiez,
                ImprimirLegal = imprimirLegal,
                CajaDefinicion = venta.CajaDefinicion,
                Referencia = ventaCreada.Codigo,
                Configuracion = venta.ConfOperacion != null ? (int)venta.ConfOperacion : null
            };

            var asientoContableCosto = new GuardarCostoAsientoContableDTO
            {
                Automatico = true,
                Moneda = ventaCreada.Moneda,
                Sucursal = ventaCreada.Sucursal,
                Factura = ventaCreada.Factura,
                Operador = ventaCreada.Vendedor,
                Fecha = ventaCreada.Fecha,
                CostoTotalCinco = costoTotalCinco,
                CostoTotalDiez = costoTotalDiez,
                CostoTotalExentas = costoTotalExentas,
                Cotizacion = cotizacionDolar != null ? cotizacionDolar.Monto : 7300,
                MonedaDolar = ventaCreada.Moneda,
                ImprimirLegal = imprimirLegal,
                Referencia = ventaCreada.Codigo
            };

            await _contabilidadService.GuardarAsientoContable(asientoContable);
            await _contabilidadService.GuardarCostoAsientoContable(asientoContableCosto);

            await _auditoriaService.RegistrarAuditoria(5, 1, (int)ventaCreada.Codigo, "Usuario Web", (int)ventaCreada.Vendedor, "Venta creada desde el sistema web");
            return ventaCreada;
        }

        public async Task<ReporteVentaAnual> GenerarReporte(ParametrosReporte parametros)
        {
            var reporte = new ReporteVentaAnual();
            var detalles = new Dictionary<int, DetalleVentaAnual>();

            if (parametros.CantidadAnios == 0)
            {
                parametros.CantidadAnios = 3;
            }

            // Cambiar la lógica: empezar desde el año más antiguo y subir
            var anioInicio = parametros.AnioInicio - parametros.CantidadAnios + 1;
            
            // Obtener datos para los años especificados
            for (int i = 0; i < parametros.CantidadAnios; i++)
            {
                var anioActual = anioInicio + i; // Ahora suma en lugar de restar
                var parametrosAnio = new ParametrosReporte
                {
                    AnioInicio = anioActual,
                    VendedorId = parametros.VendedorId,
                    CategoriaId = parametros.CategoriaId,
                    ClienteId = parametros.ClienteId,
                    MarcaId = parametros.MarcaId,
                    ArticuloId = parametros.ArticuloId,
                    CiudadId = parametros.CiudadId,
                    SucursalId = parametros.SucursalId,
                    DepositoId = parametros.DepositoId,
                    MonedaId = parametros.MonedaId,
                    ProveedorId = parametros.ProveedorId,
                    VerUtilidadYCosto = parametros.VerUtilidadYCosto,
                    MovimientoPorFecha = parametros.MovimientoPorFecha
                };

                var ventasAnio = await _ventaRepository.ObtenerVentasAnuales(parametrosAnio);
                var totalNotasCredito = await _ventaRepository.ObtenerTotalNotasCredito(parametrosAnio);
                var totalNotasCreditoSinItems = await _ventaRepository.ObtenerTotalNotasCreditoSinItems(parametrosAnio);
                var totalNotasDevolucion = await _ventaRepository.ObtenerTotalNotasDevolucion(parametrosAnio);

                foreach (var venta in ventasAnio)
                {
                    if (!detalles.ContainsKey(venta.CodigoArticulo))
                    {
                        detalles[venta.CodigoArticulo] = new DetalleVentaAnual
                        {
                            CodigoArticulo = venta.CodigoArticulo,
                            Descripcion = venta.Descripcion,
                            Stock = venta.Stock,
                            Costo = venta.Costo,
                            PrecioVenta1 = venta.PrecioVenta1,
                            PrecioVenta2 = venta.PrecioVenta2,
                            PrecioVenta3 = venta.PrecioVenta3
                        };
                    }

                    var detalle = detalles[venta.CodigoArticulo];
                    
                    // Ahora i=0 es el año más antiguo, i=1 es el siguiente, etc.
                    switch (i)
                    {
                        case 0: // Año más antiguo (Año 1 para los usuarios)
                            detalle.CantidadAnio1 = venta.Cantidad;
                            detalle.ImporteAnio1 = venta.Importe;
                            reporte.Totales.TotalCantidadAnio1 += venta.Cantidad;
                            reporte.Totales.TotalImporteAnio1 += venta.Importe;
                            reporte.Totales.TotalNotasCreditoAnio1 = totalNotasCredito + totalNotasCreditoSinItems + totalNotasDevolucion;
                            break;
                        case 1: // Segundo año más antiguo (Año 2 para los usuarios)
                            detalle.CantidadAnio2 = venta.Cantidad;
                            detalle.ImporteAnio2 = venta.Importe;
                            reporte.Totales.TotalCantidadAnio2 += venta.Cantidad;
                            reporte.Totales.TotalImporteAnio2 += venta.Importe;
                            reporte.Totales.TotalNotasCreditoAnio2 = totalNotasCredito + totalNotasCreditoSinItems + totalNotasDevolucion;
                            break;
                        case 2: // Tercer año más antiguo (Año 3 para los usuarios)
                            detalle.CantidadAnio3 = venta.Cantidad;
                            detalle.ImporteAnio3 = venta.Importe;
                            reporte.Totales.TotalCantidadAnio3 += venta.Cantidad;
                            reporte.Totales.TotalImporteAnio3 += venta.Importe;
                            reporte.Totales.TotalNotasCreditoAnio3 = totalNotasCredito + totalNotasCreditoSinItems + totalNotasDevolucion;
                            break;
                        case 3: // Cuarto año más antiguo (Año 4 para los usuarios)
                            detalle.CantidadAnio4 = venta.Cantidad;
                            detalle.ImporteAnio4 = venta.Importe;
                            reporte.Totales.TotalCantidadAnio4 += venta.Cantidad;
                            reporte.Totales.TotalImporteAnio4 += venta.Importe;
                            reporte.Totales.TotalNotasCreditoAnio4 = totalNotasCredito + totalNotasCreditoSinItems + totalNotasDevolucion;
                            break;
                        case 4: // Quinto año más antiguo (Año 5 para los usuarios)
                            detalle.CantidadAnio5 = venta.Cantidad;
                            detalle.ImporteAnio5 = venta.Importe;
                            reporte.Totales.TotalCantidadAnio5 += venta.Cantidad;
                            reporte.Totales.TotalImporteAnio5 += venta.Importe;
                            reporte.Totales.TotalNotasCreditoAnio5 = totalNotasCredito + totalNotasCreditoSinItems + totalNotasDevolucion;
                            break;
                    }
                }
            }

            // Obtener datos del rango de fechas específico para calcular VentaTotal, ImporteTotal y UnidadesVendidas
            var ventasRangoFechas = new List<DetalleVentaAnual>();
            if (parametros.FechaDesde.HasValue && parametros.FechaHasta.HasValue)
            {
                var parametrosRangoFechas = new ParametrosReporte
                {
                    AnioInicio = parametros.AnioInicio,
                    VendedorId = parametros.VendedorId,
                    CategoriaId = parametros.CategoriaId,
                    ClienteId = parametros.ClienteId,
                    MarcaId = parametros.MarcaId,
                    ArticuloId = parametros.ArticuloId,
                    CiudadId = parametros.CiudadId,
                    SucursalId = parametros.SucursalId,
                    DepositoId = parametros.DepositoId,
                    MonedaId = parametros.MonedaId,
                    ProveedorId = parametros.ProveedorId,
                    VerUtilidadYCosto = parametros.VerUtilidadYCosto,
                    MovimientoPorFecha = parametros.MovimientoPorFecha,
                    FechaDesde = parametros.FechaDesde,
                    FechaHasta = parametros.FechaHasta
                };

                ventasRangoFechas = (await _ventaRepository.ObtenerVentasAnuales(parametrosRangoFechas)).ToList();
            }

            var codigosArticulos = detalles.Keys.Select(key => (uint)key);

            var metas = new Dictionary<uint, decimal>();

            if( parametros.VendedorId.HasValue)
            {
                metas = await _metasService.GetMetasPorArticulo(codigosArticulos, parametros.AnioInicio, parametros.VendedorId.HasValue ? (uint)parametros.VendedorId.Value : null);
            }
            else
            {
                metas = await _metasService.GetMetasGeneralPorArticulo(codigosArticulos, parametros.AnioInicio);
            }

            foreach (var detalle in detalles.Values)
            {
                detalle.MetaAcordada = metas.FirstOrDefault(m => m.Key == detalle.CodigoArticulo).Value;
                
                // Calcular UnidadesVendidas, ImporteTotal y VentaTotal basados en el rango de fechas
                if (parametros.FechaDesde.HasValue && parametros.FechaHasta.HasValue)
                {
                    var ventaRangoFechas = ventasRangoFechas.FirstOrDefault(v => v.CodigoArticulo == detalle.CodigoArticulo);
                    if (ventaRangoFechas != null)
                    {
                        detalle.UnidadesVendidas = ventaRangoFechas.Cantidad;
                        detalle.ImporteTotal = ventaRangoFechas.Importe;
                        detalle.VentaTotal = detalle.MetaAcordada * detalle.PrecioVenta1;
                    }
                    else
                    {
                        detalle.UnidadesVendidas = 0;
                        detalle.ImporteTotal = 0;
                        detalle.VentaTotal = detalle.MetaAcordada * detalle.PrecioVenta1;
                    }
                }
                else
                {
                    // Si no hay rango de fechas, usar el año más reciente (último año del rango)
                    detalle.UnidadesVendidas = detalle.CantidadAnio3; // Cambiar según la cantidad de años
                    detalle.ImporteTotal = detalle.ImporteAnio3; // Cambiar según la cantidad de años
                    detalle.VentaTotal = detalle.MetaAcordada * detalle.PrecioVenta1;
                }

                // Calcular DemandaPromedio (promedio de unidades vendidas de todos los años)
                detalle.DemandaPromedio = (detalle.CantidadAnio1 + detalle.CantidadAnio2 + detalle.CantidadAnio3 + detalle.CantidadAnio4 + detalle.CantidadAnio5) / parametros.CantidadAnios;

                // Calcular PorcentajeUtilidad (porcentaje de beneficio basado en el precio de venta y el costo)
                detalle.PorcentajeUtilidad = (detalle.PrecioVenta1 - detalle.Costo) / (detalle.PrecioVenta1 == 0 ? 1 : detalle.PrecioVenta1);
            }

            reporte.Detalles = [.. detalles.Values];
            return reporte;
        }

        public async Task<IEnumerable<VentaViewModel>> GetVentasPorCliente(string clienteRuc)
        {
            var cliente = await _clienteRepository.GetByRuc(clienteRuc) ?? throw new Exception("Cliente no encontrado");
            var ventas = await _ventaRepository.ConsultaVentas(
                null, // fecha_desde 
                null, // fecha_hasta
                null, // sucursal
                cliente.Codigo, // cliente
                null, // vendedor
                null, // articulo
                null, // moneda
                null, // factura
                null, // venta
                null, // estadoVenta
                null, // remisiones
                null, // listaFacturasSinCDC
                null, // page
                3); // itemsPorPagina
            return ventas;
        }


        public async Task<ReporteVentaPorProveedorViewModel> GetReporteVentasPorProveedor(string fechaDesde, string fechaHasta, uint? proveedor, uint? cliente)
        {
            var datos = await _proveedoresRepository.GetReporteProveedores(fechaDesde, fechaHasta, proveedor, cliente);
            
            if (proveedor.HasValue)
            {
                var TotalCompras = await _ventaRepository.ObtenerTotalComprasProveedor(fechaDesde, fechaHasta, proveedor, cliente);
                var TotalPagado = await _ventaRepository.ObtenerTotalPagadoProveedor(fechaDesde, fechaHasta, proveedor, cliente);
                var TotalNotasDebito = await _ventaRepository.ObtenerTotalNotasDebitoProveedor(proveedor);
                var Saldo = TotalCompras - TotalPagado;
                if (Saldo > 0)
                {
                    Saldo = Saldo + TotalNotasDebito;
                }
                Saldo = Math.Max(0, Saldo);

                var reporte = new ReporteVentaPorProveedorViewModel
                {
                    ReporteVentasPorProveedor = datos,
                    TotalCompras = TotalCompras,
                    TotalPagado = TotalPagado,
                    Saldo = Saldo
                };
                return reporte;
            }
            return new ReporteVentaPorProveedorViewModel
            {
                ReporteVentasPorProveedor = datos,
                TotalCompras = 0,
                TotalPagado = 0,
                Saldo = 0
            };
        }

    }
}