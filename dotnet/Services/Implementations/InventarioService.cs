using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Models.Dtos;
using Api.Models.ViewModels;
using Api.Models.Dtos.ArticuloLote;
using Api.Models.Dtos.Articulo;
namespace Api.Services.Implementations
{
    public class InventarioService : IInventarioService
    {
        private readonly IInventarioRepository _inventarioRepository;
        private readonly IDetalleInventarioRepository _detalleInventarioRepository;
        private readonly IArticuloLoteRepository _articuloLoteRepository;
        private readonly IArticuloRepository _articuloRepository;


        public InventarioService(
            IInventarioRepository inventarioRepository,
            IDetalleInventarioRepository detalleInventarioRepository,
            IArticuloLoteRepository articuloLoteRepository,
            IArticuloRepository articuloRepository
        )
        {
            _inventarioRepository = inventarioRepository;
            _detalleInventarioRepository = detalleInventarioRepository;
            _articuloLoteRepository = articuloLoteRepository;
            _articuloRepository = articuloRepository;
        }

        public async Task<InventarioAuxiliar> AnularInventario(uint id)
        {
            var inventarioAAnular = await _inventarioRepository.GetById(id) ?? throw new InvalidOperationException($"No se encontró un inventario con el id {id}.");
            if (inventarioAAnular.Estado == 2)
            {
                throw new InvalidOperationException("El inventario ya esta anulado");
            }

            if (inventarioAAnular.Autorizado == 1)
            {
                throw new InvalidOperationException("No se puede anular un inventario ya autorizado, primero revierta la operacion");
            }

            inventarioAAnular.Estado = 2;
            await _inventarioRepository.UpdateInventario(inventarioAAnular);
            return inventarioAAnular;
        }

        public async Task<InventarioAuxiliar> CerrarInventario(uint id, uint userId)
        {

            var inventarioACerrar = await _inventarioRepository.GetById(id) ?? throw new InvalidOperationException($"No se encontró un inventario con el id {id}.");

            if (userId != inventarioACerrar.Operador)
            {
                throw new InvalidOperationException($"Solo el operador que inicio el inventario puede cerrarlo.");
            }

            if (inventarioACerrar.Estado == 2)
            {
                throw new InvalidOperationException($"No se puede cerrar un inventario ya anulado.");
            }

            if (inventarioACerrar.Estado == 1)
            {
                throw new InvalidOperationException($"El inventario ya esta cerrado.");
            }

            if (inventarioACerrar.Autorizado == 1)
            {
                throw new InvalidOperationException($"No se puede cerrar un inventario previamente autorizado.");
            }

            inventarioACerrar.Estado = 1;
            await _inventarioRepository.UpdateInventario(inventarioACerrar);
            return inventarioACerrar;
        }

        public async Task<InventarioAuxiliar> AutorizarInventario(
            uint id
        )
        {
            var inventarioAAutorizar = await _inventarioRepository.GetById(id) ?? throw new InvalidOperationException($"No se encontró un inventario con el id {id}.");

            if (inventarioAAutorizar.Autorizado == 1)
            {
                throw new InvalidOperationException($"Inventario ya fue autorizado previamente.");
            }

            if (inventarioAAutorizar.Estado == 2)
            {
                throw new InvalidOperationException($"No se puede autorizar un inventario anulado.");
            }
            if (inventarioAAutorizar.Estado == 0)
            {
                throw new InvalidOperationException($"No se puede autorizar un inventario anulado.");
            }
            
            inventarioAAutorizar.Autorizado = 1;
            await _inventarioRepository.UpdateInventario(inventarioAAutorizar);

            var detallesInventario = await _detalleInventarioRepository.GetByInventario(inventarioAAutorizar.Id);

            foreach (InventarioAuxiliarItems item in detallesInventario)
            {
                var lote = await _articuloLoteRepository.GetById(item.IdLote);
                var articulo = await _articuloRepository.GetById(item.IdArticulo);
                if (lote != null)
                {
                    var patchDto = new ArticuloLotePatchDTO
                    {
                        AlCantidad = item.CantidadFinal,  // Actualizamos la cantidad según el inventario
                    };

                    await _articuloLoteRepository.UpdatePartial(lote.AlCodigo, patchDto);
                }
                if (articulo != null)
                {
                    articulo.ArBloquear = 0; 
                    await _articuloRepository.Update(articulo);  //desbloqueamos el articulo para que permita movimientos de nuevo
                }
            }
            return inventarioAAutorizar;
        }

        public async Task<InventarioAuxiliar> RevertirInventario(uint id)
        {
            var inventarioARevertir = await _inventarioRepository.GetById(id) ?? throw new InvalidOperationException($"No se encontró un inventario con el id {id}.");

            if (inventarioARevertir.Autorizado == 1)
            {
                if (inventarioARevertir.Estado == 2)
                {
                    throw new InvalidOperationException($"No se puede revertir un inventario anulado.");
                }
                ;

                if (inventarioARevertir.Estado == 0)
                {
                    throw new InvalidOperationException($"No se puede revertir un inventario aun abierto.");
                }

                inventarioARevertir.Autorizado = 0;
                await _inventarioRepository.UpdateInventario(inventarioARevertir);
                var detallesInventario = await _detalleInventarioRepository.GetByInventario(inventarioARevertir.Id);

                foreach (InventarioAuxiliarItems item in detallesInventario)
                {
                    var lote = await _articuloLoteRepository.GetById(item.IdLote);
                    if (lote != null)
                    {
                        var patchDto = new ArticuloLotePatchDTO
                        {
                            AlCantidad = item.CantidadInicial,  // Actualizamos la cantidad según el inventario
                        };

                        await _articuloLoteRepository.UpdatePartial(lote.AlCodigo, patchDto);
                    }
                }
                return inventarioARevertir;
            }
            else if (inventarioARevertir.Autorizado == 0)
            {
                throw new InvalidOperationException($"No se puede revertir un inventario que aun no ha sido autorizado.");

            }
            return inventarioARevertir;
        }

        public async Task<InventarioAuxiliarItems> InsertarItems(InsertarItemsDTO item, uint idInventario)
        {

            var existeInventario = await _inventarioRepository.GetById(idInventario) ?? throw new InvalidOperationException($"No se encontró un inventario con el id {idInventario}.");

            if (existeInventario.Estado >= 1)
            {
                throw new InvalidOperationException("El inventario está cerrado o anulado.");
            }

            if (existeInventario.Autorizado == 1)
            {
                throw new InvalidOperationException("El inventario ya fue autorizado.");
            }

            // primero verificamos si ese item ya no se agrego anteriormente al inventario
            var existeLote = await _detalleInventarioRepository.BuscarLoteExistente(idInventario, item.IdLote);
            if (existeLote)
            {
                throw new InvalidOperationException("Este item ya fue agregado a este mismo inventario.");
            }

            var existeEnGeneral = await _detalleInventarioRepository.BuscarLoteExistenteGeneral(idInventario);
            if (existeEnGeneral)
            {
                throw new InvalidOperationException("Este item ya existe en algun inventario aun abierto.");
            }

            var articuloLote = await _articuloLoteRepository.GetById(item.IdLote) ?? throw new InvalidOperationException($"No se encontró el lote con el id {item.IdLote}");


            if (articuloLote.AlLote != item.Lote || articuloLote.AlCodBarra != item.CodigoBarra)
            {
                var patchDto = new ArticuloLotePatchDTO
                {
                    AlLote = item.Lote,
                    AlCodBarra = item.CodigoBarra
                };

                await _articuloLoteRepository.UpdatePartial(articuloLote.AlCodigo, patchDto);
            }

            var articuloABloquear = await _articuloRepository.GetById(item.IdArticulo) ?? throw new InvalidOperationException($"No se encontró el articulo con el id {item.IdArticulo}");

            articuloABloquear.ArBloquear = 1;

            await _articuloRepository.Update(articuloABloquear); //bloqueamos el articulo para que no tenga movimientos mientras esta en inventario

            var nuevoItem = new InventarioAuxiliarItems
            {
                IdInventario = idInventario,
                IdArticulo = item.IdArticulo,
                IdLote = item.IdLote,
                Lote = item.Lote,
                FechaVencimientoItem = item.FechaVencimientoItem,
                CantidadInicial = item.CantidadInicial,
                CantidadFinal = item.CantidadFinal
            };

            await _detalleInventarioRepository.InsertarItem(nuevoItem);

            return nuevoItem;
        }

        public async Task<IEnumerable<DetalleInventarioViewModel>> ListarItemsInventario(uint idInventario, int filtro, FiltroInventarioEnum tipo, int valor, string? busqueda, bool stock)
        {
            var inventario = await _inventarioRepository.GetById(idInventario) ?? throw new InvalidOperationException("No se ha encontrado el inventario.");
            var itemsInventario = await _detalleInventarioRepository.ListarItems(idInventario, busqueda);
            var articulosFiltrados = tipo switch
            {
                FiltroInventarioEnum.Todos => await _articuloRepository.BuscarArticulos(
                    busqueda: busqueda, deposito: inventario.Deposito, stock: stock
                ),
                FiltroInventarioEnum.Marca => await _articuloRepository.BuscarArticulos(
                    marca: (uint)valor,
                    busqueda: busqueda, deposito: inventario.Deposito, stock: stock
                ),
                FiltroInventarioEnum.Seccion => await _articuloRepository.BuscarArticulos(
                    categoria: (uint)valor,
                    busqueda: busqueda, deposito: inventario.Deposito, stock: stock
                ),
                FiltroInventarioEnum.Ubicacion => await _articuloRepository.BuscarArticulos(
                    ubicacion: (uint)valor,
                    busqueda: busqueda, deposito: inventario.Deposito, stock: stock
                ),
                FiltroInventarioEnum.Categoria => await _articuloRepository.BuscarArticulos(
                    categoria: (uint)valor,
                    busqueda: busqueda, deposito: inventario.Deposito, stock: stock
                ),
                _ => throw new ArgumentException("Tipo de filtro no válido", nameof(tipo))
            };

            if (filtro == 0) //solo trae items en inventario
            {
                return itemsInventario;
            }
            else if (filtro == 1) //trae todos los artículos filtrados
            {
                var articulosMapeados = articulosFiltrados.Select(art => new DetalleInventarioViewModel
                {
                    articulo_id = art.IdArticulo,
                    cod_interno = art.CodInterno,
                    lote_id = art.IdLote,
                    descripcion = art.Descripcion,
                    ubicacion = art.Ubicacion ?? "",
                    control_vencimiento = art.ControlVencimiento,
                    vencimiento = art.VencimientoLote ?? "",
                    sub_ubicacion = art.SubUbicacion ?? "",
                    lote = art.Lote ?? "",
                    cod_barra_articulo = art.CodigoBarra ?? "",
                    cod_barra_lote = art.CodigoBarra ?? "",
                    cantidad_inicial = art.CantidadLote,
                    cantidad_final = 0
                });
                return articulosMapeados;
            }
            else if (filtro == 2) //combina items en inventario con artículos filtrados
            {
                var articulosMapeados = articulosFiltrados.Select(art => new DetalleInventarioViewModel
                {
                    articulo_id = art.IdArticulo,
                    cod_interno = art.CodInterno,
                    lote_id = art.IdLote,
                    descripcion = art.Descripcion,
                    ubicacion = art.Ubicacion ?? "",
                    control_vencimiento = art.ControlVencimiento,
                    vencimiento = art.VencimientoLote ?? "",
                    sub_ubicacion = art.SubUbicacion ?? "",
                    lote = art.Lote ?? "",
                    cod_barra_articulo = art.CodigoBarra ?? "",
                    cod_barra_lote = art.CodigoBarra ?? "",
                    cantidad_inicial = art.CantidadLote,
                    cantidad_final = 0
                });
                // Combinar ambas colecciones usando Union
                return itemsInventario.Union(articulosMapeados);
            }
            return itemsInventario;
        }
    }
}