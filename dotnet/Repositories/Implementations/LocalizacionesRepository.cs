using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class LocalizacionesRepository : DapperRepositoryBase, ILocalizacionesRepository
    {
        private readonly ApplicationDbContext _context;

        public LocalizacionesRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<Localizacion> Crear(Localizacion localizacion)
        {
            var localizacionCreada = await _context.Localizaciones.AddAsync(localizacion);
            await _context.SaveChangesAsync();
            return localizacionCreada.Entity;
        }

        public async Task<Localizacion?> GetById(uint id)
        {
            var data = await _context.Localizaciones.FirstOrDefaultAsync(loc => loc.Id == id) ?? throw new InvalidOperationException("No se encontro una localizacion con ese id.");

            return data;
        }

        public async Task<Localizacion?> GetByAgenda(uint agenda)
        {
            var datos = await _context.Localizaciones.FirstOrDefaultAsync(loc => loc.Agenda == agenda) ?? throw new KeyNotFoundException("No se encontro ninguna localizacion con esa agenda.");

            return datos;
        }

        public async Task<Localizacion> Update(Localizacion data)
        {
            _context.Localizaciones.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }
    }
}