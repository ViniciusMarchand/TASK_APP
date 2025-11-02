
namespace api.Exceptions;

public class IdentityException  : CustomException
{
    public IdentityException ()
        : base(400, "IdentityException ", "Access is forbidden.")
    {
    }

    public IdentityException (string message)
        : base(400, "IdentityException ", message)
    {
    }

    
}

