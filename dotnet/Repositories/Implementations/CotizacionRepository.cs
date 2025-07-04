using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class CotizacionRepository : DapperRepositoryBase, ICotizacionRepository
    {
        private readonly ApplicationDbContext _context;

        public CotizacionRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cotizacion>> GetCotizacionesHoy()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            // Primero intentar obtener las de hoy
            var cotizacionesHoy = await _context.Cotizaciones
                .Where(co => co.Fecha == hoy)
                .ToListAsync();

            // Si hay cotizaciones de hoy, devolverlas
            if (cotizacionesHoy.Count != 0)
            {
                return cotizacionesHoy;
            }

            // Si no hay de hoy, obtener las últimas 3
            var ultimasCotizaciones = await _context.Cotizaciones
                .OrderByDescending(co => co.Fecha)
                .Take(3)
                .ToListAsync();

            return ultimasCotizaciones;
        }

        public async Task<Cotizacion?> GetCotizacionDolarHoy()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            // Primero intentar obtener la de hoy
            var cotizacionDolarHoy = await _context.Cotizaciones
                .Where(co => co.Fecha == hoy && co.Moneda == 2)
                .FirstOrDefaultAsync();

            // Si hay cotización del dólar de hoy, devolverla
            if (cotizacionDolarHoy != null)
            {
                return cotizacionDolarHoy;
            }

            // Si no hay de hoy, obtener la más reciente del dólar
            var ultimaCotizacionDolar = await _context.Cotizaciones
                .Where(co => co.Moneda == 2)
                .OrderByDescending(co => co.Fecha)
                .FirstOrDefaultAsync();

            return ultimaCotizacionDolar;
        }
    }
}