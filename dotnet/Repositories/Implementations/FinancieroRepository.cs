using Api.Data;
using Api.Models.Dtos;
using Api.Models.ViewModels;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Mysqlx.Datatypes;

namespace Api.Repositories.Implementations
{
    public class FinancieroRepository : DapperRepositoryBase, IFinancieroRepository
    {
        private readonly ApplicationDbContext _context;

        public FinancieroRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimbradoResult>> ObtenerDatosFacturacion(uint usuario, uint sucursal)
        {

            using var connection = GetConnection();

            string query = @"
                SELECT 
                    du.d_codigo AS D_Codigo, 
                    op.op_nombre AS Op_Nombre, 
                    op.op_codigo AS Op_Codigo,
                    dv.d_descripcion AS D_Descripcion, 
                    dv.d_nrotimbrado AS D_Nrotimbrado, 
                    dv.d_fecha_vence AS D_Fecha_Vence, 
                    dv.d_comprobante AS D_Comprobante, 
                    dv.d_p_emision AS D_P_Emision, 
                    dv.d_establecimiento AS D_Establecimiento, 
                    dv.d_nroDesde AS D_NroDesde, 
                    dv.d_nroHasta AS D_NroHasta, 
                    dv.d_nro_secuencia AS D_Nro_Secuencia 
                FROM definir_usuario du 
                LEFT JOIN operadores op ON du.d_usuario = op.op_codigo 
                LEFT JOIN definicion_ventas dv ON du.d_definicion = dv.d_codigo 
                WHERE d_activo = 1 AND op.op_codigo = @Usuario AND d_comprobante = 1";

            string queryGeneral = @"
              SELECT * FROM definicion_ventas WHERE d_activo = 1 AND d_principal = 1 AND d_sucursal = @Sucursal AND d_comprobante = 1
            ";

            var result = await connection.QueryAsync<TimbradoResult>(query, new { Usuario = usuario });
            var resultGeneral = await connection.QueryAsync<TimbradoResult>(queryGeneral, new { Sucursal = sucursal });

            if (result.AsList().Count > 0)
            {
                return result;
            }
            else
            {
                return resultGeneral;
            }

        }
    }
}