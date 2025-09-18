using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IJwtService _jwtService;

    public CreateUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validar token de administrador
        var tokenValidation = _jwtService.ValidateToken(request.Token);
        
        if (!tokenValidation.IsValid)
        {
            throw new DomainException("Invalid token");
        }

        if (!tokenValidation.IsAdministrator)
        {
            throw new DomainException("Only administrators can create users");
        }

        // Verificar se o email já existe
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new DomainException(new Dictionary<string, string>
            {
                { "Email", "Este email já está sendo usado por outro usuário" }
            });
        }

        // Hash da senha
        var (passwordHash, salt) = await _authenticationService.HashPasswordAsync(request.Password);

        // Criar usuário
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsAdministrator = false // Novos usuários não são administradores por padrão
        };

        var createdUser = await _userRepository.CreateAsync(user);

        return new UserResponse
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            CreatedAt = createdUser.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = createdUser.UpdatedAt,
            IsActive = createdUser.IsActive,
            IsAdministrator = createdUser.IsAdministrator
        };
    }
}
