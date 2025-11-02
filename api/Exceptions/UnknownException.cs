
namespace api.Exceptions;

public class UnknownException : CustomException
{
    public UnknownException()
        : base(500, "UnknownError", "An unknown error has occurred.")
    {
    }

    public UnknownException(string message)
        : base(500, "UnknownError", message)
    {
    }

    
}

