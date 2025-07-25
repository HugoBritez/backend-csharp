# Configuración del Servicio de Email

## Variables de Entorno Requeridas

Para usar el servicio de email de forma segura, configura las siguientes variables de entorno:

### Variables Obligatorias
```bash
EMAIL_USERNAME=tu_usuario_email
EMAIL_PASSWORD=tu_password_email
```

### Variables Opcionales (se usan valores por defecto si no están configuradas)
```bash
# Configuración del servidor SMTP
EMAIL_SMTP_SERVER=mail.sofmarsistema.net
EMAIL_SMTP_PORT=465
EMAIL_USE_SSL=true
EMAIL_FROM_EMAIL=prueba@sofmarsistema.net
EMAIL_FROM_NAME=Sofmar CRM Recordatorios
```

## Configuración en appsettings.json

El servicio también puede leer la configuración desde `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "mail.sofmarsistema.net",
    "SmtpPort": 465,
    "UseSsl": true,
    "FromEmail": "prueba@sofmarsistema.net",
    "FromName": "Sofmar CRM Recordatorios"
  }
}
```

## Prioridad de Configuración

1. **Variables de entorno** (máxima prioridad)
2. **appsettings.json** (fallback)

## Uso del Servicio

```csharp
// Inyección de dependencias
public class MiController : ControllerBase
{
    private readonly IEmailService _emailService;

    public MiController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<IActionResult> EnviarEmail()
    {
        var resultado = await _emailService.SendEmail(
            to: "destinatario@ejemplo.com",
            subject: "Asunto del email",
            body: "Contenido del email"
        );

        return Ok(resultado);
    }
}
```

## Seguridad

- ✅ Las credenciales se leen desde variables de entorno
- ✅ Manejo de errores implementado
- ✅ Configuración flexible por entorno
- ✅ No hay credenciales hardcodeadas en el código

## Troubleshooting

Si el email no se envía:

1. Verifica que las variables de entorno estén configuradas
2. Revisa los logs de la aplicación
3. Confirma que el servidor SMTP esté accesible
4. Valida las credenciales del servidor de email 