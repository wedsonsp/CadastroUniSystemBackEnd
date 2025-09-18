using Microsoft.EntityFrameworkCore;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Infrastructure.Persistence;

namespace Sistemaws.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    public UserRepository(SistemawsDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.Deleted);
    }

    public async Task<User> CreateAsync(User user)
    {
        await AddAsync(user);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        await base.UpdateAsync(user);
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            await base.DeleteAsync(user);
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email && !u.Deleted);
    }
}