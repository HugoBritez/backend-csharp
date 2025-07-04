using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services.Implementations
{
    public class MetasArticulosService : IMetasService
    {
        private readonly IMetaVentaRepository _metaVentaRepository;
        private readonly IMetaGeneralRepository _metaGeneralRepository;

        public MetasArticulosService(IMetaVentaRepository metaVentaRepository, IMetaGeneralRepository metaGeneralRepository)
        {
            _metaVentaRepository = metaVentaRepository;
            _metaGeneralRepository = metaGeneralRepository;
        }

        public async Task<Dictionary<uint, decimal>> GetMetasPorArticulo(IEnumerable<uint> articulosIds, int anio, uint? operadorId)
        {
            var metas = await _metaVentaRepository.GetMetasEnPeriodo(anio);

            if (operadorId.HasValue)
            {
                return metas
                    .Where(m => articulosIds.Contains(m.ArticuloId) && m.OperadorId == operadorId)
                    .ToDictionary(m => m.ArticuloId, m => m.MetaAcordada);
            }
            return metas
                .Where(m => articulosIds.Contains(m.ArticuloId))
                .GroupBy(m => m.ArticuloId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(m => m.MetaAcordada)
                );
        }

        public async Task<Dictionary<uint, decimal>> GetMetasGeneralPorArticulo(IEnumerable<uint> articulosIds, int anio)
        {
            var metasGenerales = await _metaGeneralRepository.GetMetasGeneralEnPeriodo(anio);

            return metasGenerales.Where(m => articulosIds.Contains(m.ArCodigo))
                .GroupBy(m => m.ArCodigo)
                .ToDictionary(g => g.Key, g => g.Sum(m => m.MetaGeneral));
        }
    }
}