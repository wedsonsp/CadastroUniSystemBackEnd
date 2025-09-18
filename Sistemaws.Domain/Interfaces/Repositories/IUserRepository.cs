using Sistemaws.Domain.Entities;

namespace Sistemaws.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, int>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}
