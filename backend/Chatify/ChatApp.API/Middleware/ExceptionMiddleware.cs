namespace ChatApp.API.Middleware
{
    using System.Net;
    using System.Text.Json;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred."); // Log the exception
                await HandleExceptionAsync(context, ex); // Handle and transform the exception response
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set default values for response
            var code = HttpStatusCode.InternalServerError; // 500 if unhandled
            string result;

            // Customize response based on exception type
            switch (exception)
            {
                case InvalidOperationException invalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = invalidOperationException.Message });
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { error = unauthorizedAccessException.Message });
                    break;

                // You can add more cases for different exception types if needed
                default:
                    result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result); // Return the JSON error response
        }
    }
}
