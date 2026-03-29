using ProServi.Domain.Entities;

namespace ProServi.Api;

/// <summary>
/// Adapter para mapear la IUserRepository de Infrastructure a la de Application
/// </summary>
public class UserRepositoryAdapter : ProServi.Application.Services.IUserRepository
{
    private readonly ProServi.Infrastructure.Repositories.IUserRepository _infraRepo;

    public UserRepositoryAdapter(ProServi.Infrastructure.Repositories.IUserRepository infraRepo)
    {
        _infraRepo = infraRepo;
    }

    public async Task<User> GetByEmailAsync(string email) => await _infraRepo.GetByEmailAsync(email);
    public async Task<User> GetByPhoneAsync(string phone) => await _infraRepo.GetByPhoneAsync(phone);
    public async Task<bool> EmailExistsAsync(string email) => await _infraRepo.EmailExistsAsync(email);
    public async Task<User> GetByIdAsync(int id) => await _infraRepo.GetByIdAsync(id);
    public async Task AddAsync(User entity) => await _infraRepo.AddAsync(entity);
    public async Task UpdateAsync(User entity) => await _infraRepo.UpdateAsync(entity);
    public async Task SaveAsync() => await _infraRepo.SaveAsync();
}
