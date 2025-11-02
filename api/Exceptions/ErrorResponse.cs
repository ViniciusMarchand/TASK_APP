namespace api.Exceptions;

public class ErrorResponse(int statusCode, Exception ex, string traceId)
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = ex.Message;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string TraceId { get; set; } = traceId;
}