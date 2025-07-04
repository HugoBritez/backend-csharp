using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class InventarioRepository : DapperRepositoryBase, IInventarioRepository
    {
        private readonly ApplicationDbContext _context;

        public InventarioRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<InventarioAuxiliar> CrearInventario(InventarioAuxiliar inventario)
        {
            var inventarioACrear = await _context.InventariosAuxiliares.AddAsync(inventario);
            await _context.SaveChangesAsync();
            return inventarioACrear.Entity;
        }

        public async Task<InventarioAuxiliar?> UpdateInventario(InventarioAuxiliar inventario)
        {
            var inventarioACerrar = await _context.InventariosAuxiliares.FirstOrDefaultAsync(i => i.Id == inventario.Id);

            if (inventarioACerrar == null)
            {
                return null;
            }

            _context.Entry(inventarioACerrar).CurrentValues.SetValues(inventario);
            await _context.SaveChangesAsync();

            return inventario;
        }

        public async Task<InventarioAuxiliar?> GetById(uint id)
        {
            var inventario = await _context.InventariosAuxiliares.FirstOrDefaultAsync(i => i.Id == id);
            return inventario;
        }

        public async Task<IEnumerable<InventarioViewModel>> ListarInventarios(
            uint? estado,
            uint? deposito,
            string? nro_inventario
        )
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var where = "WHERE 1=1 ";

            if (estado.HasValue)
            {
                where += " AND ia.estado = @estado";
                parameters.Add("estado", estado);
            }

            if (deposito.HasValue)
            {
                where += " AND ia.deposito = @deposito";
                parameters.Add("@deposito", deposito);
            }
            if (!string.IsNullOrEmpty(nro_inventario))
            {
                where += " AND ia.nro_inventario = @nro_inventario";
                parameters.Add("@nro_inventario", nro_inventario);
            }


            var query =
            @$"
               SELECT  
                ia.id,
                ia.fecha as fecha_inicio,
                TIME_FORMAT(ia.hora, '%H:%i') as hora_inicio,
                ia.fecha_cierre,
                TIME_FORMAT(ia.hora_cierre, '%H:%i') as hora_cierre,
                ia.operador as operador_id,
                op.op_nombre as operador_nombre,
                ia.sucursal as sucursal_id,
                su.descripcion as sucursal_nombre,
                ia.deposito as deposito_id,
                de.dep_descripcion as deposito_nombre,
                ia.nro_inventario,
                ia.estado,
                ia.autorizado
              FROM inventario_auxiliar ia
              INNER JOIN operadores op ON ia.operador = op.op_codigo
              INNER JOIN sucursales su ON ia.sucursal = su.id
              INNER JOIN depositos de ON ia.deposito = de.dep_codigo
              {where}
              ORDER BY ia.fecha DESC, ia.hora DESC";


            Console.WriteLine(query);
            return await connection.QueryAsync<InventarioViewModel>(query, parameters);
        }
    }
}