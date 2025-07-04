using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TipoDocumentoRepository : DapperRepositoryBase, ITipoDocumentoRepository
    {

        public readonly ApplicationDbContext _context;
        public TipoDocumentoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoDocumento>> GetAll()
        {
            var documentos = await _context.TipoDocumentos.Where(doc => doc.Estado == 1).ToListAsync();
            if (documentos == null || documentos.Count == 0)
            {
                return [];
            }

            return documentos;
        }
    }
}