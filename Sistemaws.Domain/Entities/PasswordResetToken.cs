using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Domain.Entities;

public class PasswordResetToken : BaseEntity<int>
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpiresAt { get; set; }
    
    public bool IsUsed { get; set; } = false;
    
    // Navigation property
    public User User { get; set; } = null!;
}
