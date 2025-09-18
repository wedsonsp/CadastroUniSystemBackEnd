using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Domain.DTOs;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    public string Email { get; set; } = string.Empty;
}
