using api.Models;

namespace api.Services.Interfaces;

public interface ITokenGeneratorService
{
    string GenerateToken(User user, string role);
}