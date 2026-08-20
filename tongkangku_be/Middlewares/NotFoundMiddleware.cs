namespace tongkangku_be.Middlewares
{
    using global::tongkangku_be.Shared;
    using System.Text.Json;

    namespace tongkangku_be.Middleware
    {
        public class NotFoundMiddleware
        {
            private readonly RequestDelegate _next;

            public NotFoundMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status404NotFound
                    && !context.Response.HasStarted)
                {
                    var response = ApiResponse<object>.ErrorResult(
                        "Endpoint tidak ditemukan",
                        "ROUTE_NOT_FOUND",
                        new
                        {
                            method = context.Request.Method,
                            path = context.Request.Path.Value
                        }
                    );

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(response)
                    );
                }
            }
        }
    }
}
