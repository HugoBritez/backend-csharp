using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Models.ViewModels;
using Api.Models.Entities;

namespace Api.Services.Implementations
{
    public class NotificationServices : BackgroundService
    {
        private readonly ILogger<NotificationServices> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(15); // Verificar cada 15 minutos para mayor precisión

        public NotificationServices(
            ILogger<NotificationServices> logger, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de notificaciones iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessNotificationsAsync();
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Servicio de notificaciones cancelado");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el procesamiento de notificaciones");
                    await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken); // Esperar 15 minutos antes de reintentar
                }
            }

            _logger.LogInformation("Servicio de notificaciones detenido");
        }

        private async Task ProcessNotificationsAsync()
        {
            _logger.LogDebug("Iniciando procesamiento de notificaciones");

            // Crear un scope para acceder a servicios scoped
            using var scope = _serviceScopeFactory.CreateScope();
            var recordatorioCRMRepository = scope.ServiceProvider.GetRequiredService<IRecordatorioCRMRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();

            try
            {
                var now = DateTime.Now;
                var recordatorios = await recordatorioCRMRepository.GetAll();

                if (!recordatorios.Any())
                {
                    _logger.LogDebug("No hay recordatorios para procesar");
                    return;
                }

                _logger.LogDebug("Total de recordatorios obtenidos: {Count}", recordatorios.Count());

                // VENTANA DE TIEMPO INTELIGENTE - CORREGIDA
                var recordatoriosHoy = recordatorios
                    .Where(r => 
                        r.FechaLimite.Date == now.Date && // Misma fecha
                        r.Estado == 0 &&
                        r.Enviado == 0) // Solo recordatorios no enviados
                    .ToList();

                _logger.LogDebug("Recordatorios para hoy: {Count}", recordatoriosHoy.Count);

                // Filtrar por hora específica del recordatorio
                var recordatoriosEnVentana = recordatoriosHoy
                    .Where(r => 
                    {
                        // Crear DateTime combinando la fecha límite con la hora específica
                        var horaRecordatorio = r.Hora;
                        var fechaHoraRecordatorio = r.FechaLimite.Date.Add(horaRecordatorio);
                        
                        // Ventana: desde 5 minutos antes hasta 30 minutos después
                        var inicioVentana = now.AddMinutes(-5);
                        var finVentana = now.AddMinutes(30);
                        
                        var enVentana = fechaHoraRecordatorio >= inicioVentana && fechaHoraRecordatorio <= finVentana;
                        
                        _logger.LogDebug("Recordatorio {Id}: Fecha={Fecha}, Hora={Hora}, FechaHora={FechaHora}, EnVentana={EnVentana}", 
                            r.Codigo, r.FechaLimite.Date, horaRecordatorio, fechaHoraRecordatorio, enVentana);
                        
                        return enVentana;
                    })
                    .ToList();

                _logger.LogInformation("Procesando {Count} recordatorios en ventana de tiempo", recordatoriosEnVentana.Count);

                foreach (var recordatorio in recordatoriosEnVentana)
                {
                    try
                    {
                        await ProcessSingleNotificationAsync(recordatorio, emailService, usuarioRepository);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando recordatorio ID: {RecordatorioId}", recordatorio.Codigo);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo recordatorios");
                throw;
            }
        }

        private async Task ProcessSingleNotificationAsync(
            RecordatorioCRMViewModel recordatorio, 
            IEmailService emailService, 
            IUsuarioRepository usuarioRepository)
        {
            try
            {
                // Obtener el email del operador
                var operador = await usuarioRepository.GetOperadorById(recordatorio.Operador);
                
                if (operador == null || string.IsNullOrEmpty(operador.OpEmail))
                {
                    _logger.LogWarning("Operador {OperadorId} no encontrado o sin email configurado", recordatorio.Operador);
                    return;
                }

                _logger.LogInformation("Enviando notificación a: {Email}", operador.OpEmail);

                // Crear el mensaje del recordatorio
                var mensaje = CreateNotificationMessage(recordatorio);

                var emailEnviado = await emailService.SendEmail(
                    operador.OpEmail, 
                    $"Recordatorio CRM: {recordatorio.Titulo}", 
                    mensaje);

                if (emailEnviado)
                {
                    _logger.LogInformation("Notificación enviada exitosamente a: {Email}", operador.OpEmail);
                    
                    // Marcar recordatorio como procesado
                    await _serviceScopeFactory.CreateScope()
                        .ServiceProvider.GetRequiredService<IRecordatorioCRMRepository>()
                        .MarcarComoEnviado(recordatorio.Codigo);
                }
                else
                {
                    _logger.LogError("No se pudo enviar la notificación a: {Email}", operador.OpEmail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email para recordatorio ID: {RecordatorioId}", recordatorio.Codigo);
                throw;
            }
        }

        private string CreateNotificationMessage(RecordatorioCRMViewModel recordatorio)
        {
            // Crear DateTime combinando la fecha límite con la hora específica
            var fechaHoraRecordatorio = recordatorio.FechaLimite.Date.Add(recordatorio.Hora);
            
            var mensaje = $@"
                {recordatorio.Titulo}
                
                Descripción: {recordatorio.Descripcion}
                
                Fecha y hora del recordatorio: {fechaHoraRecordatorio:dd/MM/yyyy HH:mm}
                
                Cliente: {recordatorio.ClienteNombre ?? "No especificado"}
                
                Este es un recordatorio automático del sistema CRM de Sofmar.
                
                ---";

            return mensaje.Trim();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deteniendo servicio de notificaciones");
            await base.StopAsync(cancellationToken);
        }
    }
}