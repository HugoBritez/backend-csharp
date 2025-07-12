using Api.Auth.Models;
using MySqlConnector;
using Dapper;
using System.Text.Json;
using Api.Audit.Services;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;

namespace Api.Auth.Services
{
    public class AuthService : DapperRepositoryBase, IAuthService
    {
        private readonly IJwtService _jwtService;

        private readonly IProveedoresRepository _proveedoresRepository;

        private readonly IAuditoriaService _auditoriaService;


        public AuthService(IConfiguration configuration, IJwtService jwtService, IAuditoriaService auditoriaService, IProveedoresRepository proveedoresRepository) : base(configuration)
        {
            _jwtService = jwtService;
            _auditoriaService = auditoriaService;
            _proveedoresRepository = proveedoresRepository;
        }


        public async Task<LoginProveedorResponse> LoginProveedor(string Email, string Ruc)
        {
            try
            {
                /*
                Usuario ingresa Email + RUC
                        ↓
                    Se busca proveedor por RUC
                        ↓
                    ¿El proveedor tiene Key (código secreto)?
                        ↓ SÍ → ¿Email coincide con Key o con Mail?
                            ↓ SÍ → Acceso permitido
                            ↓ NO → Error: "Email o código de acceso incorrecto"
                        ↓ NO → Error: "Proveedor no tiene acceso al sistema"
                */
                
                int esAdmin = 0;
                var parameters = new DynamicParameters();
                parameters.Add("Email", Email);
                parameters.Add("Ruc", Ruc);

                var proveedorALoggear = await _proveedoresRepository.GetByRuc(Ruc) ?? throw new Exception("Proveedor no encontrado");

                // Verificar si el proveedor tiene un key (código secreto) - REQUERIDO para cualquier tipo de acceso
                if (string.IsNullOrEmpty(proveedorALoggear.Key))
                {
                    throw new Exception("Proveedor no tiene acceso al sistema");
                }

                // Verificar si el usuario está intentando usar código secreto (admin) o email normal
                if (proveedorALoggear.Key == Email)
                {
                    // Usuario está usando código secreto - es admin
                    esAdmin = 1;
                }
                else if (proveedorALoggear.Mail == Email)
                {
                    // Usuario está usando email correcto - no es admin
                    esAdmin = 0;
                }
                else
                {
                    // Email/código no coincide con ninguno de los dos
                    throw new Exception("Email o código de acceso incorrecto");
                }

                using var connection = await GetConnectionAsync();
                var query = @"
                  SELECT
                    pro.pro_codigo,
                    pro.pro_razon
                  FROM proveedores pro
                  WHERE pro.pro_ruc = @Ruc
                ";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query, parameters);

                if (result == null)
                    throw new Exception("RUC incorrecto");

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

                if (esAdmin == 1)
                {
                    response.ProEsAdmin = 1;
                }
                else
                {
                    response.ProEsAdmin = 0;
                }

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