using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Sistemaws.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Sistemaws.Function;

public class TestAuthFunction
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<TestAuthFunction> _logger;

    public TestAuthFunction(IUserRepository userRepository, ILogger<TestAuthFunction> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [FunctionName("TestAuth")]
    public async Task<IActionResult> TestAuth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test-auth")] HttpRequest req)
    {
        try
        {
            // Buscar o usuário admin
            var user = await _userRepository.GetByEmailAsync("admin@admin.com.br");
            
            if (user == null)
            {
                return new OkObjectResult(new { 
                    message = "Usuário admin não encontrado",
                    usersCount = await _userRepository.CountAsync()
                });
            }

            // Testar hash da senha
            var testPassword = "123456";
            var hashedPassword = HashPassword(testPassword, user.Salt);
            var isValid = hashedPassword == user.PasswordHash;

            return new OkObjectResult(new
            {
                message = "Usuário admin encontrado",
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    isActive = user.IsActive,
                    isAdministrator = user.IsAdministrator,
                    createdAt = user.CreatedAt
                },
                passwordTest = new
                {
                    testPassword = testPassword,
                    userSalt = user.Salt,
                    userHash = user.PasswordHash,
                    calculatedHash = hashedPassword,
                    isValid = isValid
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no teste de autenticação");
            return new StatusCodeResult(500);
        }
    }

    private string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }
}
