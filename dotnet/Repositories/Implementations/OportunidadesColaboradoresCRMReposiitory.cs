using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels.CRM;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class ProyectosColaboradoresCRMRepository (ApplicationDbContext context) : IProyectosColaboradoresRepositoryCRM
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ProyectosColaboradoresCRM?> GetById(uint id)
        {
            return await _context.ProyectosColaboradoresCRM.FindAsync(id);
        }

        public async Task<IEnumerable<ProyectosColaboradoresCRM>> GetByOperador(uint operadorId)
        {
            // No, hasta aquí no es correcto. Falta finalizar la expresión LINQ y materializar la consulta.
            var data = await _context.ProyectosColaboradoresCRM
                .Where(pro => pro.Colaborador == operadorId)
                .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<ColaboradoresViewModel>> GetByProyecto(uint proyectoId)
        {
            var data = await _context.ProyectosColaboradoresCRM
                .Where(pro => pro.Proyecto == proyectoId && pro.Estado == 1)
                .Join(_context.Operadores,
                    pro => (int)pro.Colaborador,
                    op => op.OpCodigo,
                    (pro, op) => new ColaboradoresViewModel
                    {
                        Codigo = pro.Codigo,
                        Proyecto = pro.Proyecto,
                        Colaborador = pro.Colaborador,
                        Estado = pro.Estado,
                        Nombre = op.OpNombre
                    })
                .ToListAsync();
            return data;
        }
        
        // Método adicional para obtener todos (incluyendo inactivos) si es necesario
        public async Task<IEnumerable<ProyectosColaboradoresCRM>> GetByProyectoIncluyendoInactivos(uint proyectoId)
        {
            var data = await _context.ProyectosColaboradoresCRM
                .Where(pro => pro.Proyecto == proyectoId)
                .ToListAsync();
            return data;
        }

        public async Task<ProyectosColaboradoresCRM> Create(ProyectosColaboradoresCRM data)
        {
            var dataCreada = await _context.ProyectosColaboradoresCRM.AddAsync(data);
            await _context.SaveChangesAsync();
            return dataCreada.Entity;
        }

        public async Task<ProyectosColaboradoresCRM> Update(ProyectosColaboradoresCRM data)
        {
            var dataActualizada = _context.ProyectosColaboradoresCRM.Update(data);
            await _context.SaveChangesAsync();
            return dataActualizada.Entity;
        }
        
        public async Task<bool> SoftDeleteByProyecto(uint proyectoId)
        {
            var colaboradores = await _context.ProyectosColaboradoresCRM
                .Where(pc => pc.Proyecto == proyectoId && pc.Estado == 1)
                .ToListAsync();
                
            foreach (var colaborador in colaboradores)
            {
                colaborador.Estado = 0; // Soft delete
            }
            
            if (colaboradores.Any())
            {
                await _context.SaveChangesAsync();
            }
            
            return true;
        }
        
        public async Task<bool> SoftDeleteByProyectoAndColaborador(uint proyectoId, uint colaboradorId)
        {
            var colaborador = await _context.ProyectosColaboradoresCRM
                .FirstOrDefaultAsync(pc => pc.Proyecto == proyectoId && pc.Colaborador == colaboradorId);
                
            if (colaborador != null)
            {
                colaborador.Estado = 0; // Soft delete
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
        
        public async Task<bool> ActivarColaborador(uint proyectoId, uint colaboradorId)
        {
            var colaborador = await _context.ProyectosColaboradoresCRM
                .FirstOrDefaultAsync(pc => pc.Proyecto == proyectoId && pc.Colaborador == colaboradorId);
                
            if (colaborador != null)
            {
                colaborador.Estado = 1; // Activar
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
    }
}