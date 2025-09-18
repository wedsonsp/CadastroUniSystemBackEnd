using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Domain.DTOs;

public class ResetPasswordWithResetTokenRequest
{
    [Required(ErrorMessage = "Token de reset é obrigatório")]
    public string ResetToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nova senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
    public string NewPassword { get; set; } = string.Empty;
}
