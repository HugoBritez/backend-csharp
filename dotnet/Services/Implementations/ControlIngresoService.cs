using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Models.Dtos;
using Api.Models.ViewModels;
using Api.Models.Dtos.ArticuloLote;
using Api.Models.Dtos.Articulo;
using Microsoft.Extensions.Logging;
using System;

namespace Api.Services.Implementations
{
    public class ControlIngresoService : IControlIngresoService
    {
        private readonly IComprasRepository _comprasRepository;
        private readonly IDetalleComprasRepository _detalleComprasRepository;
        private readonly ITransferenciasRepository _transferenciasRepository;
        private readonly ITransferenciasItemsRepository _transferenciasItemsRepository;
        private readonly ITransferenciasItemsVencimientoRepository _transferenciasItemsVencimientoRepository;
        private readonly IArticuloLoteRepository _articuloLoteRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly IDetalleComprasVencimientoRepository _detalleComprasVencimientoRepository;
        private readonly ILogger<ControlIngresoService> _logger;

        public ControlIngresoService(
            IComprasRepository comprasRepository,
            IDetalleComprasRepository detalleComprasRepository,
            ITransferenciasRepository transferenciasRepository,
            ITransferenciasItemsRepository transferenciasItemsRepository,
            ITransferenciasItemsVencimientoRepository transferenciasItemsVencimientoRepository,
            IArticuloLoteRepository articuloLoteRepository,
            IArticuloRepository articuloRepository,
            IDetalleComprasVencimientoRepository detalleComprasVencimientoRepository,
            ILogger<ControlIngresoService> logger
        )
        {
            _comprasRepository = comprasRepository;
            _detalleComprasRepository = detalleComprasRepository;
            _transferenciasRepository = transferenciasRepository;
            _transferenciasItemsRepository = transferenciasItemsRepository;
            _transferenciasItemsVencimientoRepository = transferenciasItemsVencimientoRepository;
            _articuloLoteRepository = articuloLoteRepository;
            _articuloRepository = articuloRepository;
            _detalleComprasVencimientoRepository = detalleComprasVencimientoRepository;
            _logger = logger;
        }

        public async Task<bool> VerificarCompra(uint idCompra, uint userId)
        {
            var compra = await _comprasRepository.GetById(idCompra) ?? throw new InvalidOperationException("Compra no encontrada.");

            compra.Verificado = 1;
            compra.Verificador = (int)userId;

            await _comprasRepository.Update(compra);

            return true;
        }

        public async Task<bool> VerificarItem(uint detalle, decimal cantidad, string lote, DateTime vencimiento)
        {
            _logger.LogInformation("INICIO VerificarItem - Detalle: {Detalle}, Cantidad: {Cantidad}, Lote: {Lote}, Vencimiento: {Vencimiento}", 
                detalle, cantidad, lote, vencimiento.ToString("yyyy-MM-dd"));

            try
            {
                // 1. Obtener datos necesarios
                _logger.LogInformation("PASO 1: Obteniendo datos de verificación...");
                var (detalleCompra, articulo, detalleCompraVencimiento) = await ObtenerDatosVerificacion(detalle);
                
                _logger.LogInformation("Datos obtenidos - Artículo: {ArticuloId} ({ArticuloNombre}), Control Vencimiento: {ControlVencimiento}", 
                    articulo.ArCodigo, articulo.ArDescripcion, articulo.ArVencimiento == 1 ? "SÍ" : "NO");

                // 2. Actualizar detalle de compra
                _logger.LogInformation("PASO 2: Actualizando detalle de compra...");
                await ActualizarDetalleCompra(detalleCompra, cantidad, lote, vencimiento);
                _logger.LogInformation("Detalle de compra actualizado - Cantidad verificada: {CantidadVerificada}", detalleCompra.CantidadVerificada);

                // 3. Procesar lote si el artículo tiene control de vencimiento
                if (articulo.ArVencimiento == 1)
                {
                    _logger.LogInformation("PASO 3: Procesando lote con control de vencimiento...");
                    await ProcesarLoteConVencimiento(detalleCompra, articulo, detalleCompraVencimiento, lote, vencimiento);
                    _logger.LogInformation("Lote procesado exitosamente");
                }
                else
                {
                    _logger.LogInformation("Artículo sin control de vencimiento - Saltando procesamiento de lote");
                }

                _logger.LogInformation("FIN VerificarItem - Proceso completado exitosamente");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR en VerificarItem - Detalle: {Detalle}, Error: {Message}", detalle, ex.Message);
                throw;
            }
        }

        private async Task<(DetalleCompra detalleCompra, Articulo articulo, DetalleCompraVencimiento detalleCompraVencimiento)>
            ObtenerDatosVerificacion(uint detalle)
        {
            _logger.LogInformation("Buscando detalle de compra con ID: {Detalle}", detalle);
            var detalleCompra = await _detalleComprasRepository.GetById(detalle);
            
            if (detalleCompra == null)
            {
                _logger.LogError("Detalle de compra no encontrado con ID: {Detalle}", detalle);
                throw new InvalidOperationException($"Item no encontrado: {detalle}");
            }
            
            _logger.LogInformation("Detalle de compra encontrado - Artículo: {Articulo}, Cantidad original: {Cantidad}", 
                detalleCompra.Articulo, detalleCompra.Cantidad);

            _logger.LogInformation("Buscando artículo con ID: {ArticuloId}", detalleCompra.Articulo);
            var articulo = await _articuloRepository.GetById(detalleCompra.Articulo);
            
            if (articulo == null)
            {
                _logger.LogError("Artículo no encontrado con ID: {ArticuloId}", detalleCompra.Articulo);
                throw new InvalidOperationException($"Articulo no encontrado: {detalleCompra.Articulo}");
            }

            _logger.LogInformation("Buscando detalle de compra vencimiento para detalle: {Detalle}", detalle);
            var detalleCompraVencimiento = await _detalleComprasVencimientoRepository.GetByDetalleCompra(detalle);
            
            if (detalleCompraVencimiento == null)
            {
                _logger.LogError("Detalle de compra vencimiento no encontrado para detalle: {Detalle}", detalle);
                throw new InvalidOperationException($"Detalle de compra vencimiento no encontrado: {detalle}");
            }

            _logger.LogInformation("Detalle de compra vencimiento encontrado - LoteId: {LoteId}, Lote actual: {LoteActual}", 
                detalleCompraVencimiento.LoteId, detalleCompraVencimiento.Lote);

            return (detalleCompra, articulo, detalleCompraVencimiento);
        }

        private async Task ActualizarDetalleCompra(DetalleCompra detalleCompra, decimal cantidad, string lote, DateTime vencimiento)
        {
            detalleCompra.CantidadVerificada = (int)cantidad;
            detalleCompra.Lote = lote;
            detalleCompra.Vence = vencimiento;
            await _detalleComprasRepository.Update(detalleCompra);
        }

        private async Task ProcesarLoteConVencimiento(
            DetalleCompra detalleCompra,
            Articulo articulo,
            DetalleCompraVencimiento detalleCompraVencimiento,
            string lote,
            DateTime vencimiento)
        {
            _logger.LogInformation("Obteniendo lote asociado a compra con ID: {LoteId}", detalleCompraVencimiento.LoteId);
            _logger.LogInformation("Cantidad del detalle de la Compra para comparar: {DetalleCompra}", detalleCompra.Cantidad);
            var loteAsociadoACompra = await _articuloLoteRepository.GetById((uint)detalleCompraVencimiento.LoteId);
            
            if (loteAsociadoACompra == null)
            {
                _logger.LogError("Lote asociado a compra no encontrado con ID: {LoteId}", detalleCompraVencimiento.LoteId);
                throw new InvalidOperationException($"Lote no encontrado: {detalleCompraVencimiento.LoteId}");
            }

            _logger.LogInformation("Lote asociado a compra - Cantidad actual: {CantidadActual}, Lote: {LoteActual}, Vencimiento: {VencimientoActual}", 
                loteAsociadoACompra.AlCantidad, loteAsociadoACompra.AlLote, loteAsociadoACompra.AlVencimiento.ToString("yyyy-MM-dd"));

            // SOLUCIÓN: Usar Math.Round para normalizar la precisión decimal
            var cantidadLoteNormalizada = Math.Round(loteAsociadoACompra.AlCantidad, 4);
            var cantidadDetalleNormalizada = Math.Round(detalleCompra.Cantidad, 4);
            
            // LÓGICA CORREGIDA: Un lote es existente si su cantidad es mayor que la del detalle
            var esLoteExistente = cantidadLoteNormalizada > cantidadDetalleNormalizada;
            var loteCambio = loteAsociadoACompra.AlLote != lote || loteAsociadoACompra.AlVencimiento != vencimiento;

            _logger.LogInformation("Análisis de lote - Cantidad lote normalizada: {CantidadLote}, Cantidad detalle normalizada: {CantidadDetalle}, Es existente: {EsExistente}, Hay cambio: {HayCambio}", 
                cantidadLoteNormalizada, cantidadDetalleNormalizada, esLoteExistente, loteCambio);

            if (esLoteExistente && loteCambio)
            {
                _logger.LogInformation("Procesando lote existente con cambio...");
                await ProcesarLoteExistenteConCambio(detalleCompra, articulo, detalleCompraVencimiento, loteAsociadoACompra, lote, vencimiento);
            }
            else if (!esLoteExistente)
            {
                _logger.LogInformation("Procesando lote nuevo...");
                await ProcesarLoteNuevo(detalleCompra, articulo, detalleCompraVencimiento, loteAsociadoACompra, lote, vencimiento);
            }
            else
            {
                _logger.LogInformation("Lote existente sin cambios - No se requiere procesamiento");
            }
        }

        private async Task ProcesarLoteExistenteConCambio(
            DetalleCompra detalleCompra,
            Articulo articulo,
            DetalleCompraVencimiento detalleCompraVencimiento,
            ArticuloLote loteAsociadoACompra,
            string lote,
            DateTime vencimiento)
        {
            _logger.LogInformation("Buscando lote existente en depósito {Deposito} con lote {Lote} y vencimiento {Vencimiento}", 
                loteAsociadoACompra.AlDeposito, lote, vencimiento.ToString("yyyy-MM-dd"));

            var loteExistente = await _articuloLoteRepository.BuscarPorDeposito(
                articulo.ArCodigo,
                articulo.ArVencimiento,
                loteAsociadoACompra.AlDeposito,
                lote,
                vencimiento);

            if (loteExistente != null)
            {
                _logger.LogInformation("Lote existente encontrado - ID: {LoteId}, Cantidad actual: {Cantidad}", 
                    loteExistente.AlCodigo, loteExistente.AlCantidad);
                await ActualizarLoteExistente(detalleCompra, detalleCompraVencimiento, loteAsociadoACompra, loteExistente, lote, vencimiento);
            }
            else
            {
                _logger.LogInformation("No se encontró lote existente - Creando nuevo lote desde existente");
                await CrearNuevoLoteDesdeExistente(detalleCompra, detalleCompraVencimiento, loteAsociadoACompra, lote, vencimiento);
            }
        }

        private async Task ProcesarLoteNuevo(
            DetalleCompra detalleCompra,
            Articulo articulo,
            DetalleCompraVencimiento detalleCompraVencimiento,
            ArticuloLote loteAsociadoACompra,
            string lote,
            DateTime vencimiento)
        {
            var loteExistente = await _articuloLoteRepository.BuscarPorDeposito(
                articulo.ArCodigo,
                articulo.ArVencimiento,
                loteAsociadoACompra.AlDeposito,
                lote,
                vencimiento);

            if (loteExistente != null)
            {
                await ActualizarLoteExistente(detalleCompra, detalleCompraVencimiento, loteAsociadoACompra, loteExistente, lote, vencimiento);
            }
            else
            {
                await EditarLoteExistente(detalleCompra, detalleCompraVencimiento, loteAsociadoACompra, lote, vencimiento);
            }
        }

        private async Task ActualizarLoteExistente(
            DetalleCompra detalleCompra,
            DetalleCompraVencimiento detalleCompraVencimiento,
            ArticuloLote loteAsociadoACompra,
            ArticuloLote loteExistente,
            string lote,
            DateTime vencimiento)
        {
            _logger.LogInformation("Actualizando lote existente - Cantidad a agregar: {CantidadAgregar}", detalleCompra.Cantidad);
            
            // Actualizar cantidad en lote existente
            var cantidadAnterior = loteExistente.AlCantidad;
            loteExistente.AlCantidad += (int)detalleCompra.Cantidad;
            await _articuloLoteRepository.Update(loteExistente);
            _logger.LogInformation("Lote existente actualizado - Cantidad: {CantidadAnterior} -> {CantidadNueva}", 
                cantidadAnterior, loteExistente.AlCantidad);

            // Reducir cantidad en lote asociado a compra
            var cantidadAnteriorAsociado = loteAsociadoACompra.AlCantidad;
            loteAsociadoACompra.AlCantidad -= (int)detalleCompra.Cantidad;
            await _articuloLoteRepository.Update(loteAsociadoACompra);
            _logger.LogInformation("Lote asociado a compra actualizado - Cantidad: {CantidadAnterior} -> {CantidadNueva}", 
                cantidadAnteriorAsociado, loteAsociadoACompra.AlCantidad);

            // Actualizar referencia del detalle de compra vencimiento
            await ActualizarDetalleCompraVencimiento(detalleCompraVencimiento, loteExistente.AlCodigo, lote, vencimiento);
            _logger.LogInformation("Detalle de compra vencimiento actualizado - Nuevo LoteId: {LoteId}", loteExistente.AlCodigo);
        }

        private async Task CrearNuevoLoteDesdeExistente(
            DetalleCompra detalleCompra,
            DetalleCompraVencimiento detalleCompraVencimiento,
            ArticuloLote loteAsociadoACompra,
            string lote,
            DateTime vencimiento)
        {
            var loteNuevo = CrearArticuloLote(loteAsociadoACompra, lote, vencimiento, (int)detalleCompra.Cantidad);
            await _articuloLoteRepository.Create(loteNuevo);

            // Actualizar referencia del detalle de compra vencimiento
            await ActualizarDetalleCompraVencimiento(detalleCompraVencimiento, loteNuevo.AlCodigo, lote, vencimiento);

            // Reducir cantidad en lote asociado a compra
            loteAsociadoACompra.AlCantidad -= (int)detalleCompra.Cantidad;
            await _articuloLoteRepository.Update(loteAsociadoACompra);
        }

        private async Task EditarLoteExistente(
            DetalleCompra detalleCompra,
            DetalleCompraVencimiento detalleCompraVencimiento,
            ArticuloLote loteAsociadoACompra,
            string lote,
            DateTime vencimiento)
        {
            _logger.LogInformation("Editando lote existente - Lote anterior: {LoteAnterior}, Lote nuevo: {LoteNuevo}", 
                loteAsociadoACompra.AlLote, lote);
            
            // Actualizar el lote asociado a compra con los nuevos datos
            var loteAnterior = loteAsociadoACompra.AlLote;
            var vencimientoAnterior = loteAsociadoACompra.AlVencimiento;
            
            loteAsociadoACompra.AlLote = lote;
            loteAsociadoACompra.AlVencimiento = vencimiento;
            await _articuloLoteRepository.Update(loteAsociadoACompra);
            
            _logger.LogInformation("Lote editado - Lote: {LoteAnterior} -> {LoteNuevo}, Vencimiento: {VencimientoAnterior} -> {VencimientoNuevo}", 
                loteAnterior, lote, vencimientoAnterior.ToString("yyyy-MM-dd"), vencimiento.ToString("yyyy-MM-dd"));

            // Actualizar referencia del detalle de compra vencimiento (mantiene el mismo LoteId)
            await ActualizarDetalleCompraVencimiento(detalleCompraVencimiento, loteAsociadoACompra.AlCodigo, lote, vencimiento);
            _logger.LogInformation("Detalle de compra vencimiento actualizado - LoteId: {LoteId}", loteAsociadoACompra.AlCodigo);
        }

        private ArticuloLote CrearArticuloLote(ArticuloLote loteBase, string lote, DateTime vencimiento, int cantidad)
        {
            return new ArticuloLote
            {
                AlCodigo = 0,
                AlArticulo = loteBase.AlArticulo,
                AlDeposito = loteBase.AlDeposito,
                AlLote = lote,
                AlVencimiento = vencimiento,
                AlCantidad = cantidad,
                AlPreCompra = loteBase.AlPreCompra,
                AlOrigen = loteBase.AlOrigen,
                ALSerie = loteBase.ALSerie,
                AlCodBarra = loteBase.AlCodBarra,
                AlNroTalle = loteBase.AlNroTalle,
                AlColor = loteBase.AlColor,
                AlTalle = loteBase.AlTalle,
            };
        }

        private async Task ActualizarDetalleCompraVencimiento(
            DetalleCompraVencimiento detalleCompraVencimiento,
            uint loteId,
            string lote,
            DateTime vencimiento)
        {
            detalleCompraVencimiento.LoteId = (int)loteId;
            detalleCompraVencimiento.Lote = lote;
            detalleCompraVencimiento.Vence = vencimiento;
            await _detalleComprasVencimientoRepository.Update(detalleCompraVencimiento);
        }

        public async Task<bool> ConfirmarVerificacion(
            uint idCompra,
            string factura,
            uint deposito_inicial,
            uint deposito_destino,
            uint user_id,
            uint confirmador_id,
            IEnumerable<ItemConfirmarVerificacionDTO> items
        )
        {
            //1- Actualizamos el estado de la compra

            var compra = await _comprasRepository.GetById(idCompra) ?? throw new InvalidOperationException("Compra no encontrada");
            compra.Verificado = 2;
            compra.Confirmador = (int)user_id;

            await _comprasRepository.Update(compra);

            //2- Mapear la cabecera de la transferencia e insertarla

            var transferenciaAInsertar = new Transferencia
            {
                Id = 0,
                Fecha = DateTime.Now,
                Operador = user_id,
                Origen = deposito_inicial,
                Destino = deposito_destino,
                Comprobante = factura,
                Estado = 1,
                Motivo = "TRANSFERENCIA ENTRE DEPOSITOS POR VERIFICACION DE COMPRA",
                FechaOperacion = DateTime.Now,
                IdMaestro = 0,
                EstadoTransferencia = 1,
                UserAutorizador = 0,
                Talle = 0,
                Solicitud = 0
            };

            var transferencia = await _transferenciasRepository.Crear(transferenciaAInsertar);

            //3- Procesar cada item
            foreach (ItemConfirmarVerificacionDTO item in items)
            {
                
                // 3.1 Verificamos la existencia de lotes tanto en el origen como en el de destino
                var articuloAVerificar = await _articuloRepository.GetById((uint)item.IdArticulo);

                var loteOrigen = await _articuloLoteRepository.BuscarPorDeposito((uint)item.IdArticulo, articuloAVerificar.ArVencimiento, deposito_inicial, item.Lote, item.Vencimiento);

                var loteFinal = await _articuloLoteRepository.BuscarPorDeposito((uint)item.IdArticulo, articuloAVerificar.ArVencimiento, deposito_destino, item.Lote, item.Vencimiento);

                if (loteOrigen == null)
                {
                    throw new Exception($"No se encontro el lote inicial para el articulo {item.IdArticulo}");
                }


                // 3.2 procesamos las cantidades segun el caso

                if (loteFinal != null)
                {
                    decimal cantidad_final_origen = loteOrigen.AlCantidad - item.CantidadIngreso;
                    decimal cantifaf_final_destino = loteFinal.AlCantidad + item.CantidadIngreso;

                    loteOrigen.AlCantidad = cantidad_final_origen;
                    await _articuloLoteRepository.Update(loteOrigen);

                    loteFinal.AlCantidad = cantifaf_final_destino;
                    await _articuloLoteRepository.Update(loteFinal);
                }
                else
                {
                    decimal cantidad_final_origen = loteOrigen.AlCantidad - item.CantidadIngreso;
                    decimal cantidad_final_destino = item.CantidadIngreso;

                    loteOrigen.AlCantidad = cantidad_final_origen;
                    await _articuloLoteRepository.Update(loteOrigen);

                    //mapeamos para guardar en articulos lotes el nuevo ingreso
                    var loteMapeado = new ArticuloLote
                    {
                        AlCodigo = 0,
                        AlArticulo = loteOrigen.AlArticulo,
                        AlDeposito = deposito_destino,
                        AlLote = loteOrigen.AlLote,
                        AlCantidad = cantidad_final_destino,
                        AlVencimiento = loteOrigen.AlVencimiento,
                        AlPreCompra = loteOrigen.AlPreCompra,
                        AlOrigen = loteOrigen.AlOrigen,
                        ALSerie = loteOrigen.ALSerie,
                        AlCodBarra = loteOrigen.AlCodBarra,
                        AlNroTalle = loteOrigen.AlNroTalle,
                        AlColor = loteOrigen.AlColor,
                        AlTalle = loteOrigen.AlTalle,
                        AlRegistro = loteOrigen.AlRegistro
                    };

                    await _articuloLoteRepository.Create(loteMapeado);

                }

                // 3.3 mapeamos y registramos las transferencias de los items y si aplica, los vencimientos

                var transferenciaItem = new TransferenciaItem
                {
                    Id = 0,
                    Transferencia = transferencia.Id,
                    Articulo = (uint)item.IdArticulo,
                    Cantidad = item.CantidadIngreso,
                    StockActualDestino = item.CantidadIngreso
                };

                var transferenciaItemCreado = await _transferenciasItemsRepository.Crear(transferenciaItem);

                if (articuloAVerificar.ArVencimiento == 1)
                {
                    var transferenciaVencimiento = new TransferenciaItemVencimiento
                    {
                        Id = 0,
                        IdItem = transferenciaItemCreado.Id,
                        Lote = item.Lote,
                        Fecha = loteOrigen.AlVencimiento,
                        LoteId = (int)loteOrigen.AlCodigo,
                        LoteIDD = (int)loteOrigen.AlCodigo,
                    };

                    await _transferenciasItemsVencimientoRepository.Crear(transferenciaVencimiento);
                }
            }
            return true;
        }
    }
}