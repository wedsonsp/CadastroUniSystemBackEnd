using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Sistemaws.Application.Handlers;

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthenticateCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || !user.IsActive)
        {
            throw new DomainException("Invalid credentials");
        }

        var hashedPassword = HashPassword(request.Password, user.Salt);
        
        if (hashedPassword != user.PasswordHash)
        {
            throw new DomainException("Invalid credentials");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.IsAdministrator);

        return new LoginResponse
        {
            Token = token,
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive,
                IsAdministrator = user.IsAdministrator
            }
        };
    }

    private string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }
}


