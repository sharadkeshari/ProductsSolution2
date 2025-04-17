using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
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
                // Let the request pipeline continue.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details.
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Handle the exception and return a standardized error response.
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Here you can set different response codes based on exception type if required.
            var code = HttpStatusCode.InternalServerError; // 500 by default.

            var result = JsonSerializer.Serialize(new
            {
                error = "An error occurred while processing your request.",
                // You may include more error details optionally, for example:
                message = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
