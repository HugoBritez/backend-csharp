using Microsoft.EntityFrameworkCore;
using Api.Data;
using DotNetEnv;
using Api.Auth.Services;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Repositories.Interfaces;
using Api.Repositories.Implementations;
using Api.Middlewares;
using Api.Audit.Services;
using Api.Services.Interfaces;
using Api.Services.Implementations;
using Dapper;
using Api.Models.ViewModels;
using Serilog;
using Storage;

// Cargar archivo específico según entorno
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var envFile = Path.Combine("..", $".env.{environment.ToLower()}");

Console.WriteLine("=== INICIO DE CARGA DE CONFIGURACIÓN ===");
Console.WriteLine($"Entorno detectado: {environment}");
Console.WriteLine($"Directorio actual: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"Archivo buscado: {envFile}");
Console.WriteLine($"Archivo existe: {File.Exists(envFile)}");

if (File.Exists(envFile))
{
    Env.Load(envFile);
    Console.WriteLine($"[.NET API - Lógica de negocio] Cargando configuración desde: {envFile}");
    
    // Log de variables cargadas
    Console.WriteLine("=== VARIABLES CARGADAS ===");
    Console.WriteLine($"JWT_KEY: {Env.GetString("JWT_KEY")?.Substring(0, Math.Min(10, Env.GetString("JWT_KEY")?.Length ?? 0))}...");
    Console.WriteLine($"JWT_ISSUER: {Env.GetString("JWT_ISSUER")}");
    Console.WriteLine($"JWT_AUDIENCE: {Env.GetString("JWT_AUDIENCE")}");
    Console.WriteLine($"MYSQL_HOST: {Env.GetString("MYSQL_HOST")}");
    Console.WriteLine($"MYSQL_DB: {Env.GetString("MYSQL_DB")}");
    Console.WriteLine($"ASPNETCORE_URLS: {Environment.GetEnvironmentVariable("ASPNETCORE_URLS")}");
}
else
{
    Env.Load(); // Fallback al .env principal
    Console.WriteLine("[.NET API - Lógica de negocio] Cargando configuración desde: .env");
    
    // Log de variables cargadas
    Console.WriteLine("=== VARIABLES CARGADAS (FALLBACK) ===");
    Console.WriteLine($"JWT_KEY: {Env.GetString("JWT_KEY")?.Substring(0, Math.Min(10, Env.GetString("JWT_KEY")?.Length ?? 0))}...");
    Console.WriteLine($"JWT_ISSUER: {Env.GetString("JWT_ISSUER")}");
    Console.WriteLine($"JWT_AUDIENCE: {Env.GetString("JWT_AUDIENCE")}");
    Console.WriteLine($"MYSQL_HOST: {Env.GetString("MYSQL_HOST")}");
    Console.WriteLine($"MYSQL_DB: {Env.GetString("MYSQL_DB")}");
    Console.WriteLine($"ASPNETCORE_URLS: {Environment.GetEnvironmentVariable("ASPNETCORE_URLS")}");
}

Console.WriteLine("=== FIN DE CARGA DE CONFIGURACIÓN ===");

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("=== CONFIGURACIÓN DE SERVICIOS ===");
Console.WriteLine("Registrando servicios de autenticación...");

SqlMapper.AddTypeHandler(new JsonTypeHandler<List<VentaDetalle>>());
SqlMapper.AddTypeHandler(new JsonTypeHandler<List<SucursalData>>());
SqlMapper.AddTypeHandler(new JsonTypeHandler<List<ConfiguracionFacturaElectronica>>());

// Configurar la cadena de conexión usando las variables de entorno
builder.Configuration["ConnectionStrings:DefaultConnection"] =
    $"Server={Env.GetString("MYSQL_HOST")};Database={Env.GetString("MYSQL_DB")};User={Env.GetString("MYSQL_USER")};Password={Env.GetString("MYSQL_PASSWORD")};" +
    "AllowZeroDateTime=True;ConvertZeroDateTime=True;" +
    "MaxPoolSize=100;MinPoolSize=5;Pooling=true;" +
    "ConnectionTimeout=30;DefaultCommandTimeout=60;" +
    "ConnectionReset=false;AutoEnlist=false;";

// Add services to the container.
builder.Services.AddControllers();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Agregar servicios de autenticación
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        Console.WriteLine("Configurando JWT Bearer...");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Env.GetString("JWT_ISSUER"),
            ValidAudience = Env.GetString("JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Env.GetString("JWT_KEY")))
        };
        Console.WriteLine($"JWT Configurado - Issuer: {Env.GetString("JWT_ISSUER")}, Audience: {Env.GetString("JWT_AUDIENCE")}");
    });

// Registrar servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IArticuloRepository, ArticuloRepository>();
builder.Services.AddScoped<IDepositosRepository, DepositoRepository>();
builder.Services.AddScoped<ISucursalRepository, SucursalRepository>();
builder.Services.AddScoped<IListaPrecioRepository, ListaPrecioRepository>();
builder.Services.AddScoped<IMonedaRepository, MonedaRepository>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IDetalleVentaRepository, DetalleVentaRepository>();
builder.Services.AddScoped<IDetalleBonificacionRepository, DetalleBonificacionRepository>();
builder.Services.AddScoped<IDetalleArticulosEditadoRepository, DetalleArticuloEditadoRepository>();
builder.Services.AddScoped<IDetalleVentaVencimientoRepository, DetalleVencimientoRepository>();
builder.Services.AddScoped<IArticuloLoteRepository, ArticuloLoteRepository>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IAreaSecuenciaRepository, AreaSecuenciaRepository>();
builder.Services.AddScoped<IPedidosRepository, PedidosRepository>();
builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();
builder.Services.AddScoped<IDetallePedidoFaltanteRepository, DetallePedidoFaltanteRepository>();
builder.Services.AddScoped<IPedidoEstadoRepository, PedidoEstadoRepository>();
builder.Services.AddScoped<IPresupuestosRepository, PresupuestosRepository>();
builder.Services.AddScoped<IDetallePresupuestoRepository, DetallePresupuestoRepository>();
builder.Services.AddScoped<IPedidosService, PedidoService>();
builder.Services.AddScoped<IPresupuestoObservacionRepository, PresupuestoObservacionRepository>();
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IUbicacionesRepository, UbicacionesRepository>();
builder.Services.AddScoped<IProveedoresRepository, ProveedoresRepository>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadesMedidaRepository>();
builder.Services.AddScoped<ISububicacionRepository, SububicacionRepository>();
builder.Services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IDetalleInventarioRepository, DetalleInventarioRepository>();
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IComprasRepository, ComprasRepository>();
builder.Services.AddScoped<IDetalleComprasRepository, DetalleComprasRepository>();
builder.Services.AddScoped<IControlIngresoRepository, ControlIngresoRepository>();
builder.Services.AddScoped<ITransferenciasRepository, TransferenciasRepository>();
builder.Services.AddScoped<ITransferenciasItemsRepository, TransferenciasItemsRepository>();
builder.Services.AddScoped<ITransferenciasItemsVencimientoRepository, TransferenciaItemsVencimientoRepository>();
builder.Services.AddScoped<IControlIngresoService, ControlIngresoService>();
builder.Services.AddScoped<IAgendasRepository, AgendasRepository>();
builder.Services.AddScoped<IAgendasNotasRepository, AgendasNotasRepository>();
builder.Services.AddScoped<IAgendaSubvisitaRepository, AgendaSubvisitaRepository>();
builder.Services.AddScoped<ILocalizacionesRepository, LocalizacionesRepository>();
builder.Services.AddScoped<IAgendasService, AgendaService>();
builder.Services.AddScoped<IContabilidadService, ContabilidadService>();
builder.Services.AddScoped<IContabilidadRepository, ContabilidadRepository>();
builder.Services.AddScoped<ICotizacionRepository, CotizacionRepository>();
builder.Services.AddScoped<ICajaRepository, CajaRepository>();
builder.Services.AddScoped<IMetodoPagoRepository, MetodoPagoRepository>();
builder.Services.AddScoped<IFinancieroRepository, FinancieroRepository>();
builder.Services.AddScoped<IPersonalService, PersonalService>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IZonaRepository, ZonaRepository>();
builder.Services.AddScoped<ICiudadesRepository, CiudadesRepository>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IClienteGruposRepository, GrupoClienteRepository>();
builder.Services.AddScoped<ITallesRepository, TallesRepository>();
builder.Services.AddScoped<ITipoPlazoRepository, TipoPlazoRepository>();
builder.Services.AddScoped<IMetaVentaRepository, MetaVentaArticulosRepository>();
builder.Services.AddScoped<IMetasService, MetasArticulosService>();
builder.Services.AddScoped<IDetalleComprasVencimientoRepository, DetalleCompraVencimientoRepository>();
builder.Services.AddScoped<IMetaGeneralRepository, MetaVentaGeneralRepository>();
builder.Services.AddScoped<IContactosCRMRepository, ContactosCRMRepository>();
builder.Services.AddScoped<IOportunidadesCRMRepository, OportunidadesCRMRepository>();
builder.Services.AddScoped<ITareasCRMRepository, TareasCRMRepository>();
builder.Services.AddScoped<ICRMService, CRMService>();
builder.Services.AddScoped<IEstadoCRMRepository, EstadoCRMRepository>();
builder.Services.AddScoped<IAgendamientoCRMRepository, AgendamientoCRMRepository>();
builder.Services.AddScoped<IDoctoresRepository, DoctoresRepository>();
builder.Services.AddScoped<IPacientesRepository, PacientesRepository>();
builder.Services.AddScoped<IRecordatorioCRMRepository, RecordatorioCRMRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IProyectosColaboradoresRepositoryCRM, ProyectosColaboradoresCRMRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "API de Sofmar";
        document.Info.Description = "API para el proyecto web de sofmar";
    };

    config.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Ingrese 'Bearer' [espacio] y su token JWT en el campo de texto.\nEjemplo: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });

    config.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

// Configurar DbContext con opciones optimizadas
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mySqlOptions => mySqlOptions
            .EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null)
            .CommandTimeout(60) // Aumentar timeout para coincidir con la cadena de conexión
    ), 
    ServiceLifetime.Scoped
);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// ✅ Configurar Kestrel ANTES de builder.Build()
Console.WriteLine("=== CONFIGURACIÓN DE KESTREL ===");
if (environment.Equals("Production", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Configurando Kestrel para HTTPS en producción...");
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5024, listenOptions =>
        {
            var certPath = Environment.GetEnvironmentVariable("CERT_PATH") ?? "/app/certs/aspnetcert.pfx";
            var certPassword = Environment.GetEnvironmentVariable("CERT_PASSWORD") ?? "17052006";
            
            Console.WriteLine($"Buscando certificado en: {certPath}");
            Console.WriteLine($"Certificado existe: {File.Exists(certPath)}");
            
            if (File.Exists(certPath))
            {
                listenOptions.UseHttps(certPath, certPassword);
                Console.WriteLine($"Certificado HTTPS configurado: {certPath}");
            }
            else
            {
                Console.WriteLine($"Certificado no encontrado: {certPath}");
                Console.WriteLine("Usando HTTP en lugar de HTTPS");
            }
        });
    });
}
else
{
    Console.WriteLine("Entorno de desarrollo - Kestrel configurado automáticamente");
}

var app = builder.Build();

Console.WriteLine("=== CONFIGURACIÓN DE MIDDLEWARE ===");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Configurando Swagger para desarrollo...");
    app.UseOpenApi();      // Para NSwag
    app.UseSwaggerUi();   // Para NSwag
}

Console.WriteLine("Configurando CORS...");
app.UseCors("AllowAll");  // Habilitar CORS

Console.WriteLine("Configurando HTTPS redirection...");
app.UseHttpsRedirection();

Console.WriteLine("Configurando autenticación...");
app.UseAuthentication();

Console.WriteLine("Configurando middlewares personalizados...");
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ResponseWrapperMiddleware>();

Console.WriteLine("Configurando autorización...");
app.UseAuthorization();

Console.WriteLine("Configurando controladores...");
app.MapControllers();

Console.WriteLine("=== INICIO DE LA APLICACIÓN ===");
app.Run();
