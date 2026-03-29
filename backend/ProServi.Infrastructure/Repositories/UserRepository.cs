using Microsoft.EntityFrameworkCore;
using ProServi.Application.Services;
using ProServi.Domain.Entities;
using ProServi.Infrastructure.Data;

namespace ProServi.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository, ProServi.Application.Services.IUserRepository
{
    public UserRepository(ProServiDbContext context) : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Professional)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByPhoneAsync(string phone)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Professional)
            .FirstOrDefaultAsync(u => u.Phone == phone);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}
