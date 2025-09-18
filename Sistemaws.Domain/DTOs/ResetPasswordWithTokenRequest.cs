using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Domain.DTOs;

public class ResetPasswordWithTokenRequest
{
    [Required(ErrorMessage = "Token é obrigatório")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nova senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
    public string NewPassword { get; set; } = string.Empty;
}
