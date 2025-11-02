namespace api.Exceptions;

public abstract class CustomException(int StatusCode, string errorType, string message) : Exception(message)
{
    public int StatusCode { get; } = StatusCode;
    public string ErrorType { get; } = errorType;
}