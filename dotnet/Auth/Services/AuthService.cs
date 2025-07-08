using Api.Auth.Models;
using MySqlConnector;
using Dapper;
using System.Text.Json;
using Api.Audit.Services;
using Api.Repositories.Base;

namespace Api.Auth.Services
{
    public class AuthService : DapperRepositoryBase, IAuthService
    {
        private readonly IJwtService _jwtService;

        private readonly IAuditoriaService _auditoriaService;


        public AuthService(IConfiguration configuration, IJwtService jwtService, IAuditoriaService auditoriaService) : base(configuration)
        {
            _jwtService = jwtService;
            _auditoriaService = auditoriaService;
        }


        public async Task<LoginProveedorResponse> LoginProveedor(string Email, string Ruc)
        {
            try{ 
                var parameters = new DynamicParameters();
                parameters.Add("Email", Email);
                parameters.Add("Ruc", Ruc);

                using var connection = await GetConnectionAsync();
                var query = @"
                  SELECT
                    pro.pro_codigo,
                    pro.pro_razon
                  FROM proveedores pro
                  WHERE pro.pro_mail = @Email AND pro.pro_ruc = @Ruc
                ";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query, parameters);

                if (result == null)
                    throw new Exception("Email o RUC incorrectos");

                var response = new LoginProveedorResponse
                {
                    Token = _jwtService.GenerateTokenProveedor(new LoginProveedor
                    {
                        ProCodigo = Convert.ToUInt32(result.pro_codigo),
                        ProRazon = result.pro_razon
                    }),
                    Proveedor = new LoginProveedor
                    {
                        ProCodigo = Convert.ToUInt32(result.pro_codigo),
                        ProRazon = result.pro_razon
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                throw;
            }
        }

        public async Task<LoginResponse> Login(string usuario, string password)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                
                var query = @"
                    SELECT 
                        operadores.*,
                        orol.or_rol,
                        (SELECT JSON_ARRAYAGG(
                            JSON_OBJECT(
                                'menu_id', a.a_menu,
                                'menu_grupo', ms.m_grupo,
                                'menu_orden', ms.m_orden,
                                'menu_descripcion', ms.m_descripcion, 
                                'acceso', a.a_acceso,
                                'crear', a.a_agregar,
                                'editar', a.a_modificar
                            )
                        ) FROM acceso_menu_operador a
                        INNER JOIN menu_sistemas ms ON a.a_menu = ms.m_codigo 
                        WHERE a.a_operador = operadores.op_codigo) AS permisos_menu
                    FROM operadores 
                    LEFT JOIN operador_roles orol ON operadores.op_codigo = orol.or_operador
                    WHERE operadores.op_usuario = @usuario";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query, new { usuario });

                if (result == null)
                    throw new Exception("Usuario o contraseña incorrectos");

                var permisosMenu = JsonSerializer.Deserialize<List<PermisoMenu>>(result.permisos_menu);

                var usuarioResponse = new UsuarioResponse
                {
                    OpCodigo = Convert.ToUInt32(result.op_codigo),
                    OpNombre = result.op_nombre,
                    OpSucursal = Convert.ToUInt32(result.op_sucursal),
                    OpAutorizar = Convert.ToInt32(result.op_autorizar),
                    OpVerUtilidad = Convert.ToInt32(result.op_ver_utilidad),
                    OpVerProveedor = Convert.ToInt32(result.op_ver_proveedor),
                    OpAplicarDescuento = Convert.ToInt32(result.op_aplicar_descuento),
                    OpMovimiento = Convert.ToInt32(result.op_movimiento),
                    OrRol = Convert.ToInt32(result.or_rol),
                    PermisosMenu = permisosMenu
                };

                var response = new LoginResponse
                {
                    Token = _jwtService.GenerateToken(usuarioResponse),
                    Usuario = new List<UsuarioResponse> { usuarioResponse }
                };

                await _auditoriaService.RegistrarAuditoria(
                    10,
                    4,
                    (int)response.Usuario[0].OpCodigo,
                    usuario,
                    (int)response.Usuario[0].OpCodigo,
                    "Inicio de sesión desde el sistema web"
                );

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                throw;
            }
        }
    }
}