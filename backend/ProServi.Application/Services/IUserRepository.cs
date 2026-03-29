using ProServi.Domain.Entities;

namespace ProServi.Application.Services;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByPhoneAsync(string phone);
    Task<bool> EmailExistsAsync(string email);
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task SaveAsync();
}
