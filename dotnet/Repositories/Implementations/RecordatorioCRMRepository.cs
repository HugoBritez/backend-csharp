using System.Runtime.InteropServices.Marshalling;
using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class RecordatorioCRMRepository(
        ApplicationDbContext context
        ) : IRecordatorioCRMRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<RecordatorioCRMViewModel>> GetAll()
        {
            var query = from recordatorio in _context.RecordatoriosCRM
                        join usuario in _context.Operadores on (int)recordatorio.Operador equals usuario.OpCodigo
                        join cliente in _context.Clientes on recordatorio.Cliente equals cliente.Codigo
                        select new RecordatorioCRMViewModel
                        {
                            Codigo = recordatorio.Codigo,
                            Titulo = recordatorio.Titulo,
                            Descripcion = recordatorio.Descripcion,
                            Fecha = recordatorio.Fecha,
                            FechaLimite = recordatorio.FechaLimite,
                            Hora = recordatorio.Hora,
                            Operador = recordatorio.Operador,
                            Cliente = recordatorio.Cliente,
                            Estado = recordatorio.Estado,
                            TipoRecordatorio = recordatorio.TipoRecordatorio,
                            OperadorNombre = usuario.OpNombre,
                            ClienteNombre = cliente.Razon
                        };

            return await query.ToListAsync();
        }

        public async Task<RecordatorioCRMViewModel?> GetById(uint id)
        {
            var query = from recordatorio in _context.RecordatoriosCRM
                        join usuario in _context.Operadores on (int)recordatorio.Operador equals usuario.OpCodigo
                        join cliente in _context.Clientes on recordatorio.Cliente equals cliente.Codigo
                        where recordatorio.Codigo == id
                        select new RecordatorioCRMViewModel
                        {
                            Codigo = recordatorio.Codigo,
                            Titulo = recordatorio.Titulo,
                            Descripcion = recordatorio.Descripcion,
                            Fecha = recordatorio.Fecha,
                            FechaLimite = recordatorio.FechaLimite,
                            Hora = recordatorio.Hora,
                            Operador = recordatorio.Operador,
                            Cliente = recordatorio.Cliente,
                            Estado = recordatorio.Estado,
                            TipoRecordatorio = recordatorio.TipoRecordatorio,
                            OperadorNombre = usuario.OpNombre,
                            ClienteNombre = cliente.Razon
                        };

            return await query.FirstOrDefaultAsync();
        }


        public async Task<RecordatorioCRM> Create(RecordatorioCRM recordatorio)
        {
            _context.RecordatoriosCRM.Add(recordatorio);
            await _context.SaveChangesAsync();
            return recordatorio;
        }

        public async Task<RecordatorioCRM> Update(RecordatorioCRM recordatorio)
        {
            _context.RecordatoriosCRM.Update(recordatorio);
            await _context.SaveChangesAsync();
            return recordatorio;
        }
    }
}