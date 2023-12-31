using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware
{
    // Custom middleware to handle exceptions
    public class ExceptionMiddleware // W.I.P.
    {
        // This is the next middleware component in the pipeline e.g.
        // UseRouting(), UseAuthentication(), UseCors(), UseForwardHeaders(), etc.
        private readonly RequestDelegate _next;
        // Logger for logging exceptions
        private readonly ILogger<ExceptionMiddleware> _logger;
        // Provides information about the hosting environment e.g.
        // development or production
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Calls the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                // logs the middleware, sets the response content type to JSON
                // and sets the status code to 500 Internal Server Error
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Creates an error response based on the environment
                var response = _env.IsDevelopment()
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace!.ToString())
                    : new AppException(context.Response.StatusCode, "Internal Server Error");

                // Confire the options for JSON serialization,
                // serializes the error response to JSON and
                // writes it to the HTTP response stream
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }

    }
}