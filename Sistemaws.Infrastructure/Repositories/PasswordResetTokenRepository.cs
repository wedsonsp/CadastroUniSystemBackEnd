using Microsoft.EntityFrameworkCore;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Infrastructure.Persistence;

namespace Sistemaws.Infrastructure.Repositories;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly SistemawsDbContext _context;

    public PasswordResetTokenRepository(SistemawsDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        return await _context.PasswordResetTokens
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Token == token && !p.Deleted);
    }

    public async Task<PasswordResetToken?> GetActiveByUserIdAsync(int userId)
    {
        return await _context.PasswordResetTokens
            .FirstOrDefaultAsync(p => p.UserId == userId && !p.IsUsed && !p.Deleted && p.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<PasswordResetToken> CreateAsync(PasswordResetToken token)
    {
        _context.PasswordResetTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task UpdateAsync(PasswordResetToken token)
    {
        _context.PasswordResetTokens.Update(token);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var token = await _context.PasswordResetTokens.FindAsync(id);
        if (token != null)
        {
            token.Deleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CleanupExpiredTokensAsync()
    {
        var expiredTokens = await _context.PasswordResetTokens
            .Where(p => p.ExpiresAt < DateTime.UtcNow && !p.Deleted)
            .ToListAsync();

        foreach (var token in expiredTokens)
        {
            token.Deleted = true;
        }

        if (expiredTokens.Any())
        {
            await _context.SaveChangesAsync();
        }
    }
}
