using api.Enums;

namespace api.Models;

public class TaskModel : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }

    public User User { get; set; } = null!;
    public Guid UserId { get; set; }

    public bool CanEditTask(Guid userId, string role)
    {
        if (role == "admin" || role == "manager")
        {
            return true;
        }

        if (userId == UserId)
        {
            return true;
        }

        return false;
    }

    public bool CanDeleteTask(Guid userId, string role)
    {
        if (role == "admin")
        {
            return true;
        }

        if (userId == UserId)
        {
            return true;
        }

        return false;
    }
}