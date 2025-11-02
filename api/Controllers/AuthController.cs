using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.DTO;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{

    readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenDTO>> Login(UserLoginDTO dto)
    {
        AccessTokenDTO token = await _authService.Login(dto);
        return Ok(token);
    }


    [HttpPost("register")]
    public async Task<ActionResult<AccessTokenDTO>> Register(UserDTO dto)
    {
        User user = await _authService.Register(dto);
        return Ok(user);
    }

    [HttpGet("assign-role/{userId}/{role}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> AssignRole(Guid userId, string role)
    {
        bool result = await _authService.AssignRole(userId, role);
        return Ok(result);
    }

    [HttpGet("users")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> FindAllUsers()
    {
        IEnumerable<UserResponseDTO> users = await _authService.FindAllUsersAsync();
        return Ok(users);
    }
}

