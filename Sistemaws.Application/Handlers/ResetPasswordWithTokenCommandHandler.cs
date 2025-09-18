using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Application.Handlers;

public class ResetPasswordWithTokenCommandHandler : IRequestHandler<ResetPasswordWithTokenCommand, object>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IAuthenticationService _authService;

    public ResetPasswordWithTokenCommandHandler(
        IUserRepository userRepository, 
        IJwtService jwtService,
        IAuthenticationService authService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _authService = authService;
    }

    public async Task<object> Handle(ResetPasswordWithTokenCommand request, CancellationToken cancellationToken)
    {
        // Validar o token
        var tokenValidation = _jwtService.ValidateToken(request.Token);
        
        if (!tokenValidation.IsValid)
        {
            throw new DomainException("Token inválido");
        }

        // Buscar o usuário pelo ID do token
        var user = await _userRepository.GetByIdAsync(tokenValidation.UserId);
        
        if (user == null || !user.IsActive)
        {
            throw new DomainException("Usuário não encontrado ou inativo");
        }

        // Gerar novo hash da senha
        var (passwordHash, salt) = await _authService.HashPasswordAsync(request.NewPassword);
        
        // Atualizar dados do usuário
        user.PasswordHash = passwordHash;
        user.Salt = salt;
        user.UpdatedAt = DateTime.UtcNow;
        
        await _userRepository.UpdateAsync(user);
        
        return new { 
            message = "Senha atualizada com sucesso!", 
            user = new {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsAdministrator = user.IsAdministrator
            }
        };
    }
}
