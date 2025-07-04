using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Text;
using System.Text.Json;

namespace Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString("N")[..8];
            var startTime = DateTime.UtcNow;

            try
            {
                await LogRequest(context, requestId);
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(context, ex, requestId);
                throw;
            }
            finally
            {
                LogResponse(context, requestId, startTime);
            }
        }

        private async Task LogRequest(HttpContext context, string requestId)
        {
            context.Request.EnableBuffering();

            var body = await GetRequestBody(context);
            var headers = GetRelevantHeaders(context.Request.Headers);

            _logger.LogInformation(
                "REQUEST [{RequestId}] {Method} {Path} | Headers: {HeaderCount} | Body: {BodyLength} chars",
                requestId,
                context.Request.Method,
                context.Request.Path,
                headers.Count,
                body.Length
            );

            // Log body completo para debug
            if (body.Length > 0)
            {
                _logger.LogInformation("REQUEST [{RequestId}] Body: {Body}", requestId, body);
            }
        }

        private void LogResponse(HttpContext context, string requestId, DateTime startTime)
        {
            var duration = DateTime.UtcNow - startTime;
            var statusCode = context.Response.StatusCode;
            var logLevel = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

            _logger.Log(
                logLevel,
                "RESPONSE [{RequestId}] {StatusCode} | Duration: {Duration:F2}ms",
                requestId,
                statusCode,
                duration.TotalMilliseconds
            );
        }

        private void LogException(HttpContext context, Exception ex, string requestId)
        {
            var sqlDetails = ex is SqlException sqlEx 
                ? $"SQL Error {sqlEx.Number} (State: {sqlEx.State}, Server: {sqlEx.Server})"
                : null;

            _logger.LogError(
                ex,
                "ERROR [{RequestId}] {ExceptionType}: {Message} {SqlDetails}",
                requestId,
                ex.GetType().Name,
                ex.Message,
                sqlDetails ?? string.Empty
            );
        }

        private async Task<string> GetRequestBody(HttpContext context)
        {
            if (context.Request.ContentLength == 0 || !context.Request.Body.CanRead)
                return string.Empty;

            try
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                return body;
            }
            catch
            {
                return "[Error reading body]";
            }
        }

        private Dictionary<string, string> GetRelevantHeaders(IHeaderDictionary headers)
        {
            var relevantHeaders = new[]
            {
                "Authorization",
                "Content-Type",
                "User-Agent",
                "Accept",
                "Origin",
                "Referer"
            };

            return headers
                .Where(h => relevantHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(h => h.Key, h => h.Value.ToString());
        }
    }
}
