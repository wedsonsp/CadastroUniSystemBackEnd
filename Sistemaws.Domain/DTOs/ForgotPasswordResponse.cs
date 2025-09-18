namespace Sistemaws.Domain.DTOs;

public class ForgotPasswordResponse
{
    public string Message { get; set; } = string.Empty;
    public string ResetToken { get; set; } = string.Empty; // Para desenvolvimento - em produção não enviar
    public DateTime ExpiresAt { get; set; }
}
