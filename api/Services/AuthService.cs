using System.ComponentModel.DataAnnotations;
using api.DTO;
using api.Exceptions;
using api.Models;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public partial class AuthService(UserManager<User> userManager, ITokenGeneratorService tokenGeneratorService) : IAuthService
{
    readonly UserManager<User> _userManager = userManager;
    readonly ITokenGeneratorService _tokenGeneratorService = tokenGeneratorService;

    public async Task<bool> AssignRole(Guid userId, string role)
    {
        User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new EntityNotFoundException("User not found");

        if (role != "admin" && role != "manager" && role != "member")
        {
            throw new ValidationException("Invalid user role.");
        }

        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

        IdentityResult result = await _userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

        user.Role = role;
        await _userManager.UpdateAsync(user);

        return true;
    }

    public async Task<IEnumerable<UserResponseDTO>> FindAllUsersAsync()
    {
        IEnumerable<UserResponseDTO> users = await _userManager.Users
        .AsNoTracking()
        .Select(
            u => new UserResponseDTO
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role,
            }
        ).ToListAsync();

        return users;
    }

    public async Task<AccessTokenDTO> Login(UserLoginDTO dto)
    {
        User? user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new EntityNotFoundException("Invalid email or password");

        string role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "member";

        return new AccessTokenDTO
        {
            AccessToken = _tokenGeneratorService.GenerateToken(user, role)
        };
    }

    public async Task<User> Register(UserDTO dto)
    {
        EmailAddressAttribute emailValidator = new();
        if (!emailValidator.IsValid(dto.Email))
            throw new ValidationException("Invalid email format");

        User user = new()
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName
        };

        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            IEnumerable<string>? errors = result.Errors.Select(e => e.Description);
            throw new IdentityException($"Registration failed: {errors.First()}");
        }

        await _userManager.AddToRoleAsync(user, "member");

        return user;
    }
}
