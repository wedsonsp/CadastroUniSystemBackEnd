using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Application.Handlers;

public class ResetPasswordWithResetTokenCommandHandler : IRequestHandler<ResetPasswordWithResetTokenCommand, object>
{
    private readonly IPasswordResetTokenRepository _resetTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;

    public ResetPasswordWithResetTokenCommandHandler(
        IPasswordResetTokenRepository resetTokenRepository,
        IUserRepository userRepository,
        IAuthenticationService authService)
    {
        _resetTokenRepository = resetTokenRepository;
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<object> Handle(ResetPasswordWithResetTokenCommand request, CancellationToken cancellationToken)
    {
        // Buscar o token de reset
        var resetToken = await _resetTokenRepository.GetByTokenAsync(request.ResetToken);
        
        if (resetToken == null)
        {
            throw new DomainException("Token de reset inválido");
        }

        if (resetToken.IsUsed)
        {
            throw new DomainException("Token de reset já foi utilizado");
        }

        if (resetToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new DomainException("Token de reset expirado");
        }

        // Buscar o usuário
        var user = await _userRepository.GetByIdAsync(resetToken.UserId);
        
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

        // Marcar token como usado
        resetToken.IsUsed = true;
        await _resetTokenRepository.UpdateAsync(resetToken);
        
        return new { 
            message = "Senha redefinida com sucesso!", 
            user = new {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsAdministrator = user.IsAdministrator
            }
        };
    }
}
