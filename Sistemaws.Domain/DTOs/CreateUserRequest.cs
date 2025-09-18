using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Domain.DTOs;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    [MaxLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
    public string Password { get; set; } = string.Empty;
    
    public bool IsAdministrator { get; set; }
}
