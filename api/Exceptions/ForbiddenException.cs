
namespace api.Exceptions;

public class ForbiddenException  : CustomException
{
    public ForbiddenException ()
        : base(404, "ForbiddenException ", "Access is forbidden.")
    {
    }

    public ForbiddenException (string message)
        : base(404, "ForbiddenException ", message)
    {
    }

    
}

