using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Api.Middlewares
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseWrapperMiddleware> _logger;

        public ResponseWrapperMiddleware(RequestDelegate next, ILogger<ResponseWrapperMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context); // Ejecuta el pipeline

            memoryStream.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);


            // Log para debug
            _logger.LogInformation("Response Status: {StatusCode}", context.Response.StatusCode);

            // Evita formatear si es swagger, archivos, etc.
            if (context.Response.ContentType?.Contains("application/json") == true &&
                !string.IsNullOrWhiteSpace(body))
            {
                var wrapped = JsonSerializer.Serialize(new
                {
                    success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300,
                        body = JsonSerializer.Deserialize<object>(body),
                    statusCode = context.Response.StatusCode
                });

                context.Response.Body = originalBodyStream;
                context.Response.ContentLength = Encoding.UTF8.GetByteCount(wrapped);
                await context.Response.WriteAsync(wrapped);
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}