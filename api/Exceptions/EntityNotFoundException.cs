
namespace api.Exceptions;

public class EntityNotFoundException : CustomException
{
    public EntityNotFoundException()
        : base(404, "EntityNotFound", "The requested entity was not found.")
    {
    }

    public EntityNotFoundException(string message)
        : base(404, "EntityNotFound", message)
    {
    }

    
}

