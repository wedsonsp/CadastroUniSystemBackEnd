using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Application.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Buscar usuário por email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
        {
            throw new DomainException(new Dictionary<string, string>
            {
                { "Email", "Usuário não encontrado" }
            });
        }

        if (!user.IsActive)
        {
            throw new DomainException(new Dictionary<string, string>
            {
                { "User", "Usuário inativo" }
            });
        }

        // Validar senha
        var isValidPassword = await _authenticationService.ValidatePasswordAsync(request.Password, user.PasswordHash, user.Salt);
        
        if (!isValidPassword)
        {
            throw new DomainException(new Dictionary<string, string>
            {
                { "Password", "Senha inválida" }
            });
        }

        // Gerar token
        var token = await _authenticationService.GenerateTokenAsync(user);

        return new LoginResponse
        {
            Token = token,
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = user.UpdatedAt,
                IsActive = user.IsActive
            }
        };
    }
}
