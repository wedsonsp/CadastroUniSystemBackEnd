using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sistemaws.Infrastructure.Services;

public class DatabaseInitializationService
{
    private readonly SistemawsDbContext _context;
    private readonly IUserRepository _userRepository;

    public DatabaseInitializationService(SistemawsDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    public async Task InitializeAsync()
    {
        // Verificar se o banco tem usuários
        var hasUsers = await _context.Users.AnyAsync();
        
        if (!hasUsers)
        {
            await CreateDefaultAdminUser();
        }
    }

    private async Task CreateDefaultAdminUser()
    {
        // Usar o mesmo método de hash que o AuthenticationService
        var (passwordHash, salt) = await HashPasswordAsync("123456");
        
        var adminUser = new User
        {
            Name = "Administrador",
            Email = "admin@admin.com.br",
            PasswordHash = passwordHash,
            Salt = salt,
            IsActive = true,
            IsAdministrator = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1 // Auto-referência para o primeiro usuário
        };

        await _userRepository.AddAsync(adminUser);
        
        // Gerar token JWT para o usuário administrador
        var token = GenerateJwtToken(adminUser);
        
        Console.WriteLine("✅ Usuário administrador padrão criado:");
        Console.WriteLine($"   Email: admin@admin.com.br");
        Console.WriteLine($"   Senha: 123456");
        Console.WriteLine($"   Nome: Administrador");
        Console.WriteLine($"   Token JWT: {token}");
        Console.WriteLine("   ⚠️  IMPORTANTE: Use este token para autenticação inicial!");
    }

    private async Task<(string hash, string salt)> HashPasswordAsync(string password)
    {
        var salt = GenerateSalt();
        var hash = await HashPasswordWithSaltAsync(password, salt);
        return (hash, salt);
    }

    private async Task<string> HashPasswordWithSaltAsync(string password, string salt)
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

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        var jwtIssuer = "Sistemaws";
        var jwtAudience = "SistemawsUsers";
        
        var key = Encoding.UTF8.GetBytes(jwtKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("IsAdministrator", user.IsAdministrator.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(30), // Token válido por 30 dias
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
