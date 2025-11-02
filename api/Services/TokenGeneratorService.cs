using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Models;
using api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenGeneratorService(IConfiguration configuration) : ITokenGeneratorService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(User user, string role)
    {
        JwtSecurityTokenHandler? tokenHandler = new();
        byte[]? key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email!),
                new (ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new (ClaimTypes.Role, role)
            ]),
            Expires = DateTime.UtcNow.AddHours(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

