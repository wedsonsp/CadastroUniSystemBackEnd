using System.Security.Claims;
using Sistemaws.Domain.Entities;

namespace Sistemaws.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
    int? GetUserIdFromToken(string token);
}

public interface IJwtService
{
    string GenerateToken(int userId, string email, bool isAdministrator);
    JwtValidationResult ValidateToken(string token);
}

public class JwtValidationResult
{
    public bool IsValid { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsAdministrator { get; set; }
}