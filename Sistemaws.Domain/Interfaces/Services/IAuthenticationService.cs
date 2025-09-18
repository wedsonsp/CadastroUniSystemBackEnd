using Sistemaws.Domain.Entities;

namespace Sistemaws.Domain.Interfaces.Services;

public interface IAuthenticationService
{
    Task<string> AuthenticateAsync(string email, string password);
    Task<string> GenerateTokenAsync(User user);
    Task<bool> ValidatePasswordAsync(string password, string hash, string salt);
    Task<(string hash, string salt)> HashPasswordAsync(string password);
}
