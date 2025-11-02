using api.DTO;
using api.Models;

namespace api.Services.Interfaces;

public interface IAuthService
{
    Task<AccessTokenDTO> Login(UserLoginDTO dto);
    Task<User> Register(UserDTO dto);
    Task<bool> AssignRole(Guid userId, string role);
    Task<IEnumerable<UserResponseDTO>> FindAllUsersAsync();
}
