using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Sistemaws.Application.Handlers;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _resetTokenRepository;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordResetTokenRepository resetTokenRepository)
    {
        _userRepository = userRepository;
        _resetTokenRepository = resetTokenRepository;
    }

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || !user.IsActive)
        {
            // Por segurança, sempre retorna sucesso mesmo se email não existir
            return new ForgotPasswordResponse
            {
                Message = "Se o email estiver cadastrado, você receberá instruções para redefinir sua senha"
            };
        }

        // Invalidar tokens anteriores do usuário
        var existingToken = await _resetTokenRepository.GetActiveByUserIdAsync(user.Id);
        if (existingToken != null)
        {
            existingToken.IsUsed = true;
            await _resetTokenRepository.UpdateAsync(existingToken);
        }

        // Gerar novo token de reset
        var resetToken = GenerateResetToken();
        var expiresAt = DateTime.UtcNow.AddHours(1); // Token válido por 1 hora

        var passwordResetToken = new Sistemaws.Domain.Entities.PasswordResetToken
        {
            UserId = user.Id,
            Token = resetToken,
            ExpiresAt = expiresAt,
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _resetTokenRepository.CreateAsync(passwordResetToken);

        // Em produção, aqui você enviaria um email com o token
        // Por enquanto, vamos retornar o token para desenvolvimento
        Console.WriteLine($"Token de reset gerado para {request.Email}: {resetToken}");

        return new ForgotPasswordResponse
        {
            Message = "Se o email estiver cadastrado, você receberá instruções para redefinir sua senha",
            ResetToken = resetToken, // Remover em produção
            ExpiresAt = expiresAt
        };
    }

    private string GenerateResetToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
