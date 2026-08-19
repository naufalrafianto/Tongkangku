using System.Net;
using System.Text.Json;
using tongkangku_be.Shared;

namespace tongkangku_be.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ctx, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string errorCode;
            string message;
            object? details = null;

            if (exception is AppException appEx)
            {
                statusCode = appEx.StatusCode;
                errorCode = appEx.ErrorCode;
                message = appEx.Message;
                details = appEx.Details;

                _logger.LogWarning("AppException occurred: {Message} | Code: {ErrorCode}", message, errorCode);
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = "INTERNAL_SERVER_ERROR";
                message = "An unexpected error occurred on the server.";

                _logger.LogError(exception, "Unhandled Exception: {Message}", exception.Message);
            }

            context.Response.StatusCode = (int)statusCode;

            var response = ApiResponse<object>.ErrorResult(message, errorCode, details);

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }

    }
}
