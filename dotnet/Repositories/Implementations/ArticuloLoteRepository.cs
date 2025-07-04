using Api.Data;
using Api.Models.Dtos.ArticuloLote;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Repositories.Implementations
{
    public class ArticuloLoteRepository : IArticuloLoteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArticuloLoteRepository> _logger;

        public ArticuloLoteRepository(ApplicationDbContext context, ILogger<ArticuloLoteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ArticuloLote?>> GetByArticulo(uint articuloId)
        {
            return await _context.ArticuloLotes
                .Where(al => al.AlArticulo == articuloId)
                .ToListAsync();
        }

        public async Task<ArticuloLote?> GetById(uint id)
        {
            return await _context.ArticuloLotes
                .Where(al => al.AlCodigo == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ArticuloLote> Create(ArticuloLote articuloLote)
        {
            await _context.ArticuloLotes.AddAsync(articuloLote);
            await _context.SaveChangesAsync();
            return articuloLote;
        }


        public async Task<ArticuloLote?> UpdatePartial(uint id, ArticuloLotePatchDTO articuloLotePatchDTO)
        {
            var existingArticuloLote = await GetById(id);
            if (existingArticuloLote is null)
            {
                return null;
            }

            if (articuloLotePatchDTO.AlArticulo.HasValue)
            {
                existingArticuloLote.AlArticulo = articuloLotePatchDTO.AlArticulo.Value;
            }

            if (articuloLotePatchDTO.AlDeposito.HasValue)
            {
                existingArticuloLote.AlDeposito = articuloLotePatchDTO.AlDeposito.Value;
            }

            if (articuloLotePatchDTO.AlLote != null)
            {
                existingArticuloLote.AlLote = articuloLotePatchDTO.AlLote;
            }

            if (articuloLotePatchDTO.AlCantidad.HasValue)
            {
                existingArticuloLote.AlCantidad = articuloLotePatchDTO.AlCantidad.Value;
            }

            if (articuloLotePatchDTO.AlVencimiento.HasValue)
            {
                existingArticuloLote.AlVencimiento = articuloLotePatchDTO.AlVencimiento.Value;
            }

            if (articuloLotePatchDTO.AlPreCompra.HasValue)
            {
                existingArticuloLote.AlPreCompra = articuloLotePatchDTO.AlPreCompra.Value;
            }

            if (articuloLotePatchDTO.AlOrigen.HasValue)
            {
                existingArticuloLote.AlOrigen = articuloLotePatchDTO.AlOrigen.Value;
            }

            if (articuloLotePatchDTO.ALSerie != null)
            {
                existingArticuloLote.ALSerie = articuloLotePatchDTO.ALSerie;
            }

            if (articuloLotePatchDTO.AlCodBarra != null)
            {
                existingArticuloLote.AlCodBarra = articuloLotePatchDTO.AlCodBarra;
            }

            if (articuloLotePatchDTO.AlNroTalle != null)
            {
                existingArticuloLote.AlNroTalle = articuloLotePatchDTO.AlNroTalle;
            }

            if (articuloLotePatchDTO.AlColor.HasValue)
            {
                existingArticuloLote.AlColor = articuloLotePatchDTO.AlColor.Value;
            }

            if (articuloLotePatchDTO.AlTalle.HasValue)
            {
                existingArticuloLote.AlTalle = articuloLotePatchDTO.AlTalle.Value;
            }

            if (articuloLotePatchDTO.AlRegistro != null)
            {
                existingArticuloLote.AlRegistro = articuloLotePatchDTO.AlRegistro;
            }

            await _context.SaveChangesAsync();
            return existingArticuloLote;
        }

        public async Task<ArticuloLote> Update(ArticuloLote articulo)
        {
            _context.ArticuloLotes.Update(articulo);
            await _context.SaveChangesAsync();

            return articulo;
        }

        public async Task<ArticuloLote?> BuscarPorDeposito(uint id_articulo, int control_vencimiento, uint id_deposito, string lote, DateTime vencimiento)
        {
            var query = _context.ArticuloLotes
                .Where(al => al.AlArticulo == id_articulo && al.AlDeposito == id_deposito);

            if (control_vencimiento == 0)
            {
                // No agregar filtros adicionales, solo artículo y depósito
            }
            else
            {
                var vencimientoNormalizado = vencimiento.Date;
                
                _logger.LogInformation("Buscando lote - Artículo: {ArticuloId}, Depósito: {Deposito}, Lote: '{Lote}', Vencimiento: {VencimientoOriginal} -> {VencimientoNormalizado}", 
                    id_articulo, id_deposito, lote, vencimiento.ToString("yyyy-MM-dd HH:mm:ss"), vencimientoNormalizado.ToString("yyyy-MM-dd"));
                
                query = query.Where(al => al.AlLote == lote && al.AlVencimiento.Date == vencimientoNormalizado);
            }

            var resultado = await query.FirstOrDefaultAsync();
            
            if (resultado == null)
            {
                _logger.LogWarning("No se encontró lote para artículo {ArticuloId}, depósito {Deposito}, lote '{Lote}', vencimiento {Vencimiento}, control_vencimiento {ControlVencimiento}", 
                    id_articulo, id_deposito, lote, vencimiento.ToString("yyyy-MM-dd"), control_vencimiento);
                
                var lotesDisponibles = await _context.ArticuloLotes
                    .Where(al => al.AlArticulo == id_articulo && al.AlDeposito == id_deposito)
                    .Select(al => new { al.AlLote, al.AlVencimiento, al.AlCantidad })
                    .ToListAsync();
                
                _logger.LogInformation("Lotes disponibles en depósito {Deposito} para artículo {ArticuloId}:", id_deposito, id_articulo);
                foreach (var l in lotesDisponibles)
                {
                    _logger.LogInformation("  - Lote: '{Lote}', Vencimiento: {Vencimiento}, Cantidad: {Cantidad}", 
                        l.AlLote, l.AlVencimiento.ToString("yyyy-MM-dd"), l.AlCantidad);
                }
            }
            else
            {
                _logger.LogInformation("Lote encontrado - ID: {LoteId}, Lote: '{Lote}', Vencimiento: {Vencimiento}, Cantidad: {Cantidad}", 
                    resultado.AlCodigo, resultado.AlLote, resultado.AlVencimiento.ToString("yyyy-MM-dd"), resultado.AlCantidad);
            }

            return resultado;
        }

    }
}
