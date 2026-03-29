using ProServi.Domain.Entities;

namespace ProServi.Infrastructure.Repositories;

// Nota: Esta interfaz extiende IRepository para el uso interno de Infrastructure
// La interfaz pública que usa Application se encuentra en ProServi.Application.Services.IUserRepository
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByPhoneAsync(string phone);
    Task<bool> EmailExistsAsync(string email);
}
