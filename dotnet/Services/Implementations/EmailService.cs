using System.Net.Mail;
using Api.Models.Email;
using Api.Services.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Api.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            _logger.LogInformation("Iniciando envío de email a: {To}, Asunto: {Subject}", to, subject);
            
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(to, to));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                // Configuración hardcodeada para evitar problemas
                var smtpServer = "mail.sofmarsistema.net";
                var smtpPort = 465;
                var enableSsl = true;
                var username = "soporte@sofmarsistema.net";
                var password = "Soporte2025";

                _logger.LogDebug("Configurando conexión SMTP - Servidor: {SmtpServer}, Puerto: {SmtpPort}, SSL: {EnableSsl}", 
                    smtpServer, smtpPort, enableSsl);

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Configurar timeout de 30 segundos
                    client.Timeout = 30000; // 30 segundos
                    
                    // Configurar para ignorar errores de certificado SSL
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    
                    _logger.LogDebug("Intentando conectar al servidor SMTP...");
                    await client.ConnectAsync(smtpServer, smtpPort, enableSsl);
                    _logger.LogDebug("Conexión SMTP establecida exitosamente");
                    
                    _logger.LogDebug("Autenticando con usuario: {Username}", username);
                    await client.AuthenticateAsync(username, password);
                    _logger.LogDebug("Autenticación SMTP exitosa");
                    
                    _logger.LogDebug("Enviando mensaje de email");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    _logger.LogDebug("Conexión SMTP cerrada");
                }

                _logger.LogInformation("Email enviado exitosamente a: {To}", to);
                return true;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout al enviar email a {To}. El servidor SMTP no respondió en el tiempo esperado.", to);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email a {To}. Error: {ErrorMessage}", to, ex.Message);
                return false;
            }
        }
    }
}