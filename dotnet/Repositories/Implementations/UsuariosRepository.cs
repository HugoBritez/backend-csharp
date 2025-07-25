using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;

namespace Api.Repositories.Implementations
{
    public class UsuarioRepository : DapperRepositoryBase, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<Operador> CrearOperador(Operador operador)
        {
            var operadorCreado = await _context.Operadores.AddAsync(operador);
            await _context.SaveChangesAsync();
            return operadorCreado.Entity;
        }
        public async Task<IEnumerable<UsuarioViewModel>> GetUsuarios(string? busqueda, uint? id, uint? rol)
        {
            var where = "WHERE op.op_estado = 1";
            using var connection = GetConnection();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(busqueda))
            {
                where += " AND (op.op_nombre LIKE @busqueda OR op.op_documento LIKE @busqueda) LIMIT 10";
                parameters.Add("busqueda", $"%{busqueda}%");
            }
            else if (id.HasValue)
            {
                where += " AND op.op_codigo = @id";
                parameters.Add("id", id);
            }
            else if (rol.HasValue)
            {
                where += " AND rol.rol_codigo = @rol";
                parameters.Add("rol", rol);
            }
            var query = @"
              SELECT
                op.op_codigo,
                op.op_nombre,
                op.op_documento,
                rol.rol_descripcion as op_rol
                FROM operadores op
              LEFT JOIN operador_roles oprol ON op.op_codigo = oprol.or_operador
              LEFT JOIN roles rol ON oprol.or_rol = rol.rol_codigo
            " + where;
            return await connection.QueryAsync<UsuarioViewModel>(query, parameters);
        }


        public async Task<Operador?> GetOperadorById(uint id)
        {
            var operador = await _context.Operadores.FindAsync((int)id);
            return operador;
        }
    }
}