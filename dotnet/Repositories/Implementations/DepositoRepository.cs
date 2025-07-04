using Api.Models.Dtos.Deposito;
using Api.Repositories.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using Api.Repositories.Base;

namespace Api.Repositories.Implementations
{
    public class DepositoRepository : DapperRepositoryBase, IDepositosRepository
    {
        public DepositoRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<DepositoDTO>> GetDepositos(
            uint? sucursal = null,
            uint? usuario = null,
            string? descripcion = null
        )
        {
            var where = new List<string>();
            var parameters = new DynamicParameters();
            using var connection = GetConnection();


            if (sucursal.HasValue)
            {
                where.Add("dep.dep_codigo = @SucursalId");
                parameters.Add("@SucursalId", sucursal.Value);
            }

            if (usuario.HasValue)
            {
                var usuario_sucursal = await connection.QueryFirstOrDefaultAsync<uint?>(
                    "SELECT op_sucursal FROM operadores WHERE op_codigo = @UsuarioId",
                    new { UsuarioId = usuario.Value }
                );

                if (usuario_sucursal.HasValue)
                {
                    where.Add("dep.dep_codigo = @UsuarioSucursalId");
                    parameters.Add("@UsuarioSucursalId", usuario_sucursal.Value);
                }
            }

            if (!string.IsNullOrEmpty(descripcion))
            {
                where.Add("dep.dep_descripcion LIKE @Descripcion");
                parameters.Add("@Descripcion", $"%{descripcion}%");
            }

            var query = @"
                SELECT dep.dep_codigo, dep.dep_descripcion, dep_principal
                FROM depositos dep
                WHERE 1 = 1";

            if (where.Count != 0)
            {
                query += " AND " + string.Join(" AND ", where);
            }

            return await connection.QueryAsync<DepositoDTO>(query, parameters);
        }
    }
}


