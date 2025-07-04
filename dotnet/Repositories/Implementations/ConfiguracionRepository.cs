using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class ConfiguracionRepository : DapperRepositoryBase, IConfiguracionRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<Configuracion?> GetById(uint id)
        {
            var conf = await _context.Configuraciones.FirstOrDefaultAsync(cn => cn.Id == id);
            return conf;
        }
    }
}