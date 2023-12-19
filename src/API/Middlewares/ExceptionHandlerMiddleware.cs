using System.Text;
using System.Text.Json;

namespace API.Middlewares;
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var requestStr = JsonSerializer.Serialize(new
            {
                context.Request.Protocol,
                context.Request.ContentType,
                context.Request.Method,
                Path = context.Request.Path.ToString(),
            });
            _logger.LogError(ex, $"Message: {ex.Message}, Request-Info: {requestStr}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error"
            });
        }
    }
}

