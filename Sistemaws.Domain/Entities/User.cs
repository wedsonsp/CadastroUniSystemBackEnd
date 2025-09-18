using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Domain.Entities;

public class User : BaseEntity<int>
{
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required] [MaxLength(255)] public string Email { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsAdministrator { get; set; } = false;
}

