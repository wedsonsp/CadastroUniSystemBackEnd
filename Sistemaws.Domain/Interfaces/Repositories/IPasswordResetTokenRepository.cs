using Sistemaws.Domain.Entities;

namespace Sistemaws.Domain.Interfaces.Repositories;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByTokenAsync(string token);
    Task<PasswordResetToken?> GetActiveByUserIdAsync(int userId);
    Task<PasswordResetToken> CreateAsync(PasswordResetToken token);
    Task UpdateAsync(PasswordResetToken token);
    Task DeleteAsync(int id);
    Task CleanupExpiredTokensAsync();
}
