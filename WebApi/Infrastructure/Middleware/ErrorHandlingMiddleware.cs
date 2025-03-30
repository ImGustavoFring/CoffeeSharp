using System.Text.Json;

namespace WebApi.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string errorType;
            string message;

            if (exception is InvalidOperationException)
            {
                statusCode = StatusCodes.Status409Conflict;
                errorType = "Conflict";
                message = exception.Message;
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                errorType = "InternalServerError";
                message = "An unexpected error occurred";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                type = errorType,
                title = message,
                traceId = context.TraceIdentifier
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
