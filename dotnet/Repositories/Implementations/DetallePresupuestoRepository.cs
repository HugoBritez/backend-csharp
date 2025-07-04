using Dapper;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using Azure;

namespace Api.Repositories.Implementations
{
    public class DetallePresupuestoRepository : DapperRepositoryBase, IDetallePresupuestoRepository
    {
        private readonly ApplicationDbContext _context;

        public DetallePresupuestoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetallePresupuesto> GetById(uint detalleId)
        {
            var detalle = await _context.DetallePresupuesto.FirstOrDefaultAsync(depre => depre.Codigo == detalleId);
            return detalle ?? new DetallePresupuesto();
        }

        public async Task<IEnumerable<DetallePresupuesto>> GetByPresupuesto(uint pedidoId)
        {
            var detalle = await _context.DetallePresupuesto.FirstOrDefaultAsync(depre => depre.Presupuesto == pedidoId);
            return detalle != null ? new[] { detalle } : [];
        }

        public async Task<IEnumerable<DetallePresupuestoViewModel>> GetDetallePresupuesto(uint presupuestoId)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();

            var query = @"
              SELECT
                    depre.depre_codigo AS det_codigo,
                    depre.depre_articulo AS art_codigo,
                    ar.ar_codbarra AS codbarra,
                    ar.ar_descripcion AS descripcion,
                    ar.ar_editar_desc,
                    depre.depre_cantidad AS cantidad,
                    depre.depre_precio AS precio,
                    depre.depre_descuento AS descuento,
                    depre.depre_exentas AS exentas,
                    depre.depre_cinco AS cinco,
                    depre.depre_diez AS diez,
                    depre.depre_codlote AS codlote,
                    depre.depre_lote AS lote,
                    depre.depre_largura AS largura,
                    depre.depre_altura AS altura,
                    depre.depre_mts2 AS mt2,
                    depre_descripcio_art as descripcion_editada,
                    depre.depre_listaprecio AS listaprecio,
                    DATE_FORMAT(depre.depre_vence, '%Y-%m-%d') AS vence,
                    depre.depre_obs,
                    ar.ar_iva AS iva,
                    ar.ar_kilos AS kilos
                  FROM
                    detalle_presupuesto depre
                    INNER JOIN articulos ar ON ar.ar_codigo = depre.depre_articulo
                  WHERE
                    depre.depre_presupuesto = @Presupuesto
                  AND depre.depre_cantidad > 0
                  ORDER BY
                    depre.depre_codigo
            ";

            parameters.Add("@Presupuesto", presupuestoId);

            var response = await connection.QueryAsync<DetallePresupuestoViewModel>(query, parameters);

            return response;
        }

        public async Task<DetallePresupuesto> Crear(DetallePresupuesto detalle)
        {
            var detalleCreado = await _context.DetallePresupuesto.AddAsync(detalle);
            await _context.SaveChangesAsync();
            return detalleCreado.Entity ?? new DetallePresupuesto();
        }


        public async Task<DetallePresupuesto> Update(DetallePresupuesto detalle)
        {
            var detalleExistente = await _context.DetallePresupuesto.FirstOrDefaultAsync(de => de.Codigo == detalle.Codigo);

            if (detalleExistente == null)
            {
                return new DetallePresupuesto();
            }

            _context.Entry(detalleExistente).CurrentValues.SetValues(detalle);
            await _context.SaveChangesAsync();

            return detalleExistente;
        }


        //elimina los detalles de un presupuesto, no un detalle por su id
        public async Task<bool> Delete(uint idPresupuesto)
        {
            var result = await _context.DetallePresupuesto
                         .Where(dp => dp.Presupuesto == idPresupuesto)
                         .ExecuteDeleteAsync();
            return result > 0;
        }
    }
}