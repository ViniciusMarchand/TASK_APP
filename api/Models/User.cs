
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "member";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskModel> Tasks { get; set; } = null!;
    }
}