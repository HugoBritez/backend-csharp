using Api.Models.Dtos.Sucursal;
using Api.Repositories.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using Api.Repositories.Base;

namespace Api.Repositories.Implementations
{
    public class SucursalRepository : DapperRepositoryBase, ISucursalRepository
    {
        public SucursalRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<SucursalDTO>> GetSucursales(
            uint? operador = null,
            uint? matriz = null
        )
        {
            var where = new List<string>();
            var parameters = new DynamicParameters();

            if(operador.HasValue){
                where.Add("op.op_codigo = @OperadorId");
                parameters.Add("@OperadorId", operador.Value);
            }

            if(matriz.HasValue){
                where.Add("suc.matriz = @MatrizId");
                parameters.Add("@MatrizId", matriz.Value);
            }

            var query = @"
                SELECT suc.id, suc.descripcion, suc.direccion, suc.nombre_emp, suc.ruc_emp, suc.matriz
                FROM sucursales suc
                INNER JOIN operadores op ON op.op_sucursal = suc.id
                WHERE 1 = 1";

            if(where.Count != 0){
                query += " AND " + string.Join(" AND ", where);
            }

            query += " GROUP BY suc.id, suc.descripcion, suc.direccion, suc.nombre_emp, suc.ruc_emp, suc.matriz";

            using var connection = GetConnection();
            return await connection.QueryAsync<SucursalDTO>(query, parameters);
        }
    }
}
