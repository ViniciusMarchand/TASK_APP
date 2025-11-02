using System.ComponentModel.DataAnnotations;

namespace api.DTO;

public record UserDTO
{
    [Required]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required]
    public string UserName { get; init; } = string.Empty;
}
