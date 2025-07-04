using Api.Repositories.Interfaces;
using Api.Repositories.Base;
using Api.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Api.Models;
using Api.Models.ViewModels;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Repositories.Implementations
{
    public class ContabilidadRepository : DapperRepositoryBase, IContabilidadRepository
    {
        private readonly ApplicationDbContext _context;
        public ContabilidadRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<int> BuscarCodigoPlanCuentaCajaDef(uint codigoDefCaja)
        {

            using var connection = GetConnection();
            var sql = @"
              SELECT
                cp.plan
              FROM
                cajadef_plancuentas cp
                INNER JOIN cajadef cf ON cp.cajad = cf.cd_codigo
              WHERE
                cp.cajad = @codigoDefCaja
                AND cf.cd_estado = 1
            ";

            var parametro = new DynamicParameters();
            parametro.Add("codigoDefCaja", codigoDefCaja);
            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, parametro);
            if (result > 0)
            {
                return result;
            }
            else
            {
                return 0;
            }

        }

        public async Task<uint> GenerarNroAsiento()
        {
            using var connection = GetConnection();

            var query = @"
                SELECT
                    CAST(IFNULL(MAX(ac.ac_numero), 0) as char) as numero
                FROM
                    asiento_contable ac
                WHERE
                    ac.ac_cierre_asiento = 0
            ";

            var result = await connection.QueryFirstOrDefaultAsync<int>(query);
            if (result > 0)
            {
                return (uint)(result + 1);
            }
            else
            {
                return 1;
            }
        }

        public async Task<DatosCajaViewModel> GetDatosCaja(uint operadorCodigo)
        {
            using var connection = GetConnection();
            var query = @"
                SELECT
                    cd.cd_descripcion as Descripcion,
                    c.ca_fecha as Fecha,
                    c.ca_operador as Operador,
                    o.op_nombre as Cajero
                FROM
                    caja c
                    INNER JOIN cajadef cd ON c.ca_definicion = cd.cd_codigo
                    INNER JOIN operadores o ON c.ca_operador = o.op_codigo
                    LEFT JOIN caja_operador co ON co.c_caja = c.ca_codigo
                WHERE
                    (c.ca_operador = @operadorCodigo OR co.c_operador = @operadorCodigo)
                    AND c.ca_fecha = CURDATE()
                ORDER BY
                    c.ca_fecha DESC
                LIMIT 1";

            var parametros = new DynamicParameters();
            parametros.Add("operadorCodigo", operadorCodigo);

            var result = await connection.QueryFirstOrDefaultAsync<DatosCajaViewModel>(query, parametros);
            if (result != null)
            {
                return result;
            }
            else
            {
                return new DatosCajaViewModel();
            }
        }

        // public async Task<ConfiguracionAsiento> GetConfiguracionAsiento(uint NroTabla)
        // {
        //     var connection = GetConnection();

        //     var query = @"
        //       SELECT
        //         *
        //       FROM config_asiento
        //       WHERE con_nroTabla = @NroTabla
        //       AND con_estado =1
        //     ";

        //     var parametros = new DynamicParameters();
        //     parametros.Add("NroTabla", NroTabla);
        //     Console.WriteLine($"NroTabla en el repository: {NroTabla}");
        //     Console.WriteLine($"Query en el repository: {query}");
        //     var result = await connection.QueryFirstOrDefaultAsync<ConfiguracionAsiento>(query, parametros);
        //     if (result != null)
        //     {
        //         Console.WriteLine($"Configuración de asiento encontrada: {result.Exenta}");
        //         return result;
        //     }
        //     else
        //     {
        //         Console.WriteLine("No se encontró la configuración de asiento.");
        //         return new ConfiguracionAsiento();
        //     }
        // }

        public async Task<ConfiguracionAsiento> GetConfiguracionAsiento(uint NroTabla)
        {
            var configuracion =await  _context.ConfiguracionAsientos.FirstOrDefaultAsync(c => c.NroTabla == NroTabla && c.Estado == 1);
            if (configuracion != null)
            {
                return configuracion;
            }
            else
            {
                return new ConfiguracionAsiento();
            }
        }

        public decimal QuitarComas(decimal valor)
        {
            string valorString = valor.ToString();
            string valorSinComas = valorString.Replace(",", "");
            
            return decimal.Parse(valorSinComas);
        }

        public decimal RedondearNumero(decimal valor)
        {
            return Math.Round(valor, 2);
        }

        public async Task<uint> InsertarAsientoContable(AsientoContableDTO asientoContable)
        {
            using var connection = GetConnection();

            var query = @"
              INSERT INTO asiento_contable (
                    ac_sucursal,
                    ac_moneda,
                    ac_operador,
                    ac_documento,
                    ac_numero,
                    ac_fecha,
                    ac_fecha_asiento,
                    ac_totaldebe,
                    ac_totalhaber,
                    ac_cotizacion,
                    ac_referencia,
                    ac_origen,
                    ac_fecha_cierre,
                    ac_fecha_apertura,
                    ac_cotizacion_c,
                    ac_tipo_asiento,
                    ac_cierre_asiento,
                    ac_estado
                    ) VALUES (
                    @Sucursal,
                    @Moneda,
                    @Operador,
                    @Documento,
                    @Numero,
                    @Fecha,
                    @FechaAsiento,
                    @TotalDebe,
                    @TotalHaber,
                    @Cotizacion,
                    @Referencia,
                    @Origen,
                    @FechaCierre,
                    @FechaApertura,
                    @CotizacionC,
                    @TipoAsiento,
                    @CierreAsiento,
                    @Estado
              );
              SELECT LAST_INSERT_ID();
            ";

            var parametros = new DynamicParameters();
            parametros.Add("Sucursal", asientoContable.Sucursal);
            parametros.Add("Moneda", asientoContable.Moneda);
            parametros.Add("Operador", asientoContable.Operador);
            parametros.Add("Documento", asientoContable.Documento);
            parametros.Add("Numero", asientoContable.Numero);
            parametros.Add("Fecha", asientoContable.Fecha);
            parametros.Add("FechaAsiento", asientoContable.FechaAsiento);
            parametros.Add("TotalDebe", asientoContable.TotalDebe);
            parametros.Add("TotalHaber", asientoContable.TotalHaber);
            parametros.Add("Cotizacion", asientoContable.Cotizacion);
            parametros.Add("Referencia", asientoContable.Referencia);
            parametros.Add("Origen", asientoContable.Origen);
            parametros.Add("FechaCierre", asientoContable.FechaCierre);
            parametros.Add("FechaApertura", asientoContable.FechaApertura);
            parametros.Add("CotizacionC", asientoContable.CotizacionC);
            parametros.Add("TipoAsiento", asientoContable.TipoAsiento);
            parametros.Add("CierreAsiento", asientoContable.CierreAsiento);
            parametros.Add("Estado", asientoContable.Estado);
            
            try
            {
                var idResult = await connection.QueryFirstOrDefaultAsync<long>(query, parametros);
                Console.WriteLine($"ID obtenido con LAST_INSERT_ID: {idResult}");
                
                if (idResult > 0)
                {
                    return (uint)idResult;
                }
                else
                {
                    Console.WriteLine("❌ LAST_INSERT_ID devolvió 0 o null");
                    throw new Exception("No se pudo obtener el ID del asiento insertado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción durante la inserción: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Error al insertar asiento contable: {ex.Message}", ex);
            }
        }

        public async Task<DetalleAsientoContableDTO> InsertarDetalleAsientoContable(DetalleAsientoContableDTO detalleAsientoContable)
        {
            using  var connection = GetConnection();

            var query = @"
              INSERT INTO detalle_asiento_contable
                (
                  dac_asiento,
                  dac_plan,
                  dac_debe,
                  dac_haber,
                  dac_concepto
                ) VALUES (
                  @Asiento,
                  @Plan,
                  @Debe,
                  @Haber,
                  @Concepto
                )
              ";

            var parametros = new DynamicParameters();
            parametros.Add("Asiento", detalleAsientoContable.Asiento);
            parametros.Add("Plan", detalleAsientoContable.Plan);
            parametros.Add("Debe", detalleAsientoContable.Debe);
            parametros.Add("Haber", detalleAsientoContable.Haber);
            parametros.Add("Concepto", detalleAsientoContable.Concepto);
            
            var result = await connection.ExecuteAsync(query, parametros);
            if (result > 0)
            {
                return detalleAsientoContable;
            }
            else
            {
                return new DetalleAsientoContableDTO();
            }
        }
    }
}

