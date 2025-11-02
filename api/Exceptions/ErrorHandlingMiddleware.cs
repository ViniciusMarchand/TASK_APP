using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace api.Exceptions;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string traceId = Guid.NewGuid().ToString();
        
        _logger.LogError(exception, "Error trace ID: {TraceId}", traceId);

        var (statusCode, errorResponse) = exception switch
        {
            EntityNotFoundException ex => (StatusCodes.Status404NotFound,
                new ErrorResponse(404, ex, traceId)),

            ForbiddenException ex => (StatusCodes.Status403Forbidden,
                new ErrorResponse(403, ex, traceId)),

            ValidationException ex => (StatusCodes.Status400BadRequest,
                new ErrorResponse(400, ex, traceId)),

            IdentityException ex => (ex.StatusCode,
                new ErrorResponse(ex.StatusCode, ex, traceId)),

            _ => (StatusCodes.Status500InternalServerError,
                new ErrorResponse(500,
                    new UnknownException(),
                    traceId))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        string json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await context.Response.WriteAsync(json);
    }
}