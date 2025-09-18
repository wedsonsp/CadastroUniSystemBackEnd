using System.Security.Cryptography;
using System.Text;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Interfaces.Services;

namespace Sistemaws.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null || !user.IsActive)
            throw new UnauthorizedAccessException("Usuário não encontrado ou inativo");

        if (!await ValidatePasswordAsync(password, user.PasswordHash, user.Salt))
            throw new UnauthorizedAccessException("Senha inválida");

        return await GenerateTokenAsync(user);
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        return await Task.FromResult(_tokenService.GenerateToken(user));
    }

    public async Task<bool> ValidatePasswordAsync(string password, string hash, string salt)
    {
        var hashedPassword = await HashPasswordAsync(password, salt);
        return await Task.FromResult(hashedPassword == hash);
    }

    public async Task<(string hash, string salt)> HashPasswordAsync(string password)
    {
        var salt = GenerateSalt();
        var hash = await HashPasswordAsync(password, salt);
        return (hash, salt);
    }

    private async Task<string> HashPasswordAsync(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return await Task.FromResult(Convert.ToBase64String(hashedBytes));
    }

    private string GenerateSalt()
    {
        var saltBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
}