using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Persona> CreateAsync(Persona persona)
        {
            // Agregar la entidad a la base de datos
            var personaCreada = await _context.Personas.AddAsync(persona);
            // Guardar los cambios de forma asíncrona
            await _context.SaveChangesAsync();
            // Retornar la entidad creada
            return personaCreada.Entity;
        }

        public async Task<IEnumerable<Persona>> GetAll(string? Busqueda)
        {

            var query = _context.Personas.AsQueryable();

            query = query.Where(per => per.Estado == 1);

            if (!string.IsNullOrWhiteSpace(Busqueda))
            {
                var busquedaLower = Busqueda.ToLower();
                query = query.Where(per =>
                    (per.RazonSocial != null && per.RazonSocial.ToLower().Contains(busquedaLower)) ||
                    (per.NombreFantasia != null && per.NombreFantasia.ToLower().Contains(busquedaLower))
                );
            }
            return await query.ToListAsync();
        }

        public async Task<Persona?> GetByRuc(string ruc)
        {
            var persona = await _context.Personas.FirstOrDefaultAsync(per => per.Ruc == ruc);
            return persona;
        }

        public async Task<Persona> UpdatePersona(Persona data)
        {
            _context.Personas.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }


        public async Task<Persona?> GetById(uint id)
        {
            var response = await _context.Personas.FirstOrDefaultAsync(per => per.Codigo == id);
            return response;
        }

        public async Task<string?> GetUltimoCodigoInterno()
        {
            var ultimoCodigo = await _context.Clientes.Where(cli => cli.Estado == 1).MaxAsync(cli => cli.Codigo);

            ultimoCodigo += 1;
            return ultimoCodigo.ToString();
        }
    }
}