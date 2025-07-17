using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class OportunidadesCRMRepository(ApplicationDbContext context) : IOportunidadesCRMRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<OportunidadCRM> CrearOportunidad(OportunidadCRM oportunidad)
        {
            var oportunidadCreado = await _context.Oportunidades.AddAsync(oportunidad);
            await _context.SaveChangesAsync();
            return oportunidadCreado.Entity;
        }

        public async Task<OportunidadCRM> ActualizarOportunidad(OportunidadCRM oportunidad)
        {
            // Obtener la oportunidad existente de la base de datos
            var oportunidadExistente = await _context.Oportunidades.FindAsync(oportunidad.Codigo);
            
            if (oportunidadExistente == null)
            {
                throw new InvalidOperationException($"Oportunidad con código {oportunidad.Codigo} no encontrada");
            }

            // Actualizar solo los campos que no son nulos o tienen valores válidos
            if (oportunidad.Cliente > 0)
                oportunidadExistente.Cliente = oportunidad.Cliente;
            
            if (!string.IsNullOrEmpty(oportunidad.Titulo))
                oportunidadExistente.Titulo = oportunidad.Titulo;
            
            if (!string.IsNullOrEmpty(oportunidad.Descripcion))
                oportunidadExistente.Descripcion = oportunidad.Descripcion;
            
            if (oportunidad.ValorNegociacion > 0)
                oportunidadExistente.ValorNegociacion = oportunidad.ValorNegociacion;
            
            if (oportunidad.FechaInicio != default)
                oportunidadExistente.FechaInicio = oportunidad.FechaInicio;
            
            if (oportunidad.FechaFin.HasValue)
                oportunidadExistente.FechaFin = oportunidad.FechaFin;
            
            if (oportunidad.Operador > 0)
                oportunidadExistente.Operador = oportunidad.Operador;
            
            if (oportunidad.Estado > 0)
                oportunidadExistente.Estado = oportunidad.Estado;
            
            if (oportunidad.General != 0)
                oportunidadExistente.General = oportunidad.General;

            // Marcar la entidad como modificada
            _context.Oportunidades.Update(oportunidadExistente);
            await _context.SaveChangesAsync();
            
            return oportunidadExistente;
        }

        public async Task<OportunidadCRM?> GetOportunidadById(uint id)
        {
            return await _context.Oportunidades.FindAsync(id);
        }

        public async Task<IEnumerable<OportunidadCRM>> GetOportunidades()
        {
            return await _context.Oportunidades.ToListAsync();
        }

        public async Task<IEnumerable<OportunidadCRM>> GetOportunidadesByCliente(uint cliente)
        {
            return await _context.Oportunidades.Where(o => o.Cliente == cliente).ToListAsync();
        }

        public async Task<IEnumerable<OportunidadCRM>> GetOportunidadesByOperador(uint operador)
        {
            return await _context.Oportunidades.Where(o => o.Operador == operador).ToListAsync();
        }

        public async Task<OportunidadViewModel?> GetOportunidadCompletaById(uint id){
            var query = from oportunidad in _context.Oportunidades
                        join cliente in _context.ContactosCRM on oportunidad.Cliente equals cliente.Codigo
                        join operador in _context.Operadores on (int)oportunidad.Operador equals operador.OpCodigo
                        join estado in _context.EstadoCRM on oportunidad.Estado equals estado.Id
                        where oportunidad.Codigo == id
                        select new OportunidadViewModel{
                            Codigo = oportunidad.Codigo,
                            Cliente = oportunidad.Cliente,
                            Titulo = oportunidad.Titulo,
                            Descripcion = oportunidad.Descripcion,
                            ValorNegociacion = oportunidad.ValorNegociacion,
                            FechaInicio = oportunidad.FechaInicio,
                            FechaFin = oportunidad.FechaFin,
                            Operador = oportunidad.Operador,
                            Estado = oportunidad.Estado,
                            General = oportunidad.General,
                            ClienteNombre = cliente.Nombre,
                            OperadorNombre = operador.OpNombre,
                            EstadoDescripcion = estado.Descripcion
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidadesCompletas(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from oportunidad in _context.Oportunidades
                        join cliente in _context.ContactosCRM on oportunidad.Cliente equals cliente.Codigo
                        join operador in _context.Operadores on (int)oportunidad.Operador equals operador.OpCodigo
                        join estado in _context.EstadoCRM on oportunidad.Estado equals estado.Id
                        select new OportunidadViewModel
                        {
                            Codigo = oportunidad.Codigo,
                            Cliente = oportunidad.Cliente,
                            Titulo = oportunidad.Titulo,
                            Descripcion = oportunidad.Descripcion,
                            ValorNegociacion = oportunidad.ValorNegociacion,
                            FechaInicio = oportunidad.FechaInicio,
                            FechaFin = oportunidad.FechaFin,
                            Operador = oportunidad.Operador,
                            Estado = oportunidad.Estado,
                            General = oportunidad.General,
                            ClienteNombre = cliente.Nombre,
                            OperadorNombre = operador.OpNombre,
                            EstadoDescripcion = estado.Descripcion
                        };

            // Aplicar filtros de fecha si están especificados
            if (fechaInicio.HasValue)
            {
                query = query.Where(o => o.FechaInicio >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(o => o.FechaInicio <= fechaFin.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidadesCompletasByCliente(uint cliente)
        {
            var query = from oportunidad in _context.Oportunidades
                        join clienteEntity in _context.ContactosCRM on oportunidad.Cliente equals clienteEntity.Codigo
                        join operador in _context.Operadores on (int)oportunidad.Operador equals operador.OpCodigo
                        join estado in _context.EstadoCRM on oportunidad.Estado equals estado.Id
                        where oportunidad.Cliente == cliente
                        select new OportunidadViewModel
                        {
                            Codigo = oportunidad.Codigo,
                            Cliente = oportunidad.Cliente,
                            Titulo = oportunidad.Titulo,
                            Descripcion = oportunidad.Descripcion,
                            ValorNegociacion = oportunidad.ValorNegociacion,
                            FechaInicio = oportunidad.FechaInicio,
                            FechaFin = oportunidad.FechaFin,
                            Operador = oportunidad.Operador,
                            Estado = oportunidad.Estado,
                            General = oportunidad.General,
                            ClienteNombre = clienteEntity.Nombre,
                            OperadorNombre = operador.OpNombre,
                            EstadoDescripcion = estado.Descripcion
                        };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<OportunidadViewModel>> GetOportunidadesCompletasByOperador(uint operador)
        {
            var query = from oportunidad in _context.Oportunidades
                        join cliente in _context.ContactosCRM on oportunidad.Cliente equals cliente.Codigo
                        join operadorEntity in _context.Operadores on (int)oportunidad.Operador equals operadorEntity.OpCodigo
                        join estado in _context.EstadoCRM on oportunidad.Estado equals estado.Id
                        where oportunidad.Operador == operador
                        select new OportunidadViewModel
                        {
                            Codigo = oportunidad.Codigo,
                            Cliente = oportunidad.Cliente,
                            Titulo = oportunidad.Titulo,
                            Descripcion = oportunidad.Descripcion,
                            ValorNegociacion = oportunidad.ValorNegociacion,
                            FechaInicio = oportunidad.FechaInicio,
                            FechaFin = oportunidad.FechaFin,
                            Operador = oportunidad.Operador,
                            Estado = oportunidad.Estado,
                            General = oportunidad.General,
                            ClienteNombre = cliente.Nombre,
                            OperadorNombre = operadorEntity.OpNombre,
                            EstadoDescripcion = estado.Descripcion
                        };

            return await query.ToListAsync();
        }
    }
}