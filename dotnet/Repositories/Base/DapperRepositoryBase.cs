using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Api.Repositories.Base
{
    public abstract class DapperRepositoryBase
    {
        protected readonly string _connectionString;
        protected readonly ILogger? _logger;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        // Constructor compatible hacia atrás
        public DapperRepositoryBase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La cadena de conexión 'DefaultConnection' no fue encontrada");
            _logger = null;
        }

        // Nuevo constructor con logger
        public DapperRepositoryBase(IConfiguration configuration, ILogger logger)
            : this(configuration)
        {
            _logger = logger;
        }

        protected async Task<IDbConnection> GetConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                _logger?.LogDebug("Conexión Dapper abierta: {ConnectionId}", connection.GetHashCode());
                return connection;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al abrir conexión Dapper");
                connection?.Dispose();
                throw;
            }
        }

        protected IDbConnection GetConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            try
            {
                connection.Open();
                _logger?.LogDebug("Conexión Dapper abierta: {ConnectionId}", connection.GetHashCode());
                return connection;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al abrir conexión Dapper");
                connection?.Dispose();
                throw;
            }
        }

        protected void LogConnectionClosed(IDbConnection connection)
        {
            _logger?.LogDebug("Conexión Dapper cerrada: {ConnectionId}", connection.GetHashCode());
        }
    }
}