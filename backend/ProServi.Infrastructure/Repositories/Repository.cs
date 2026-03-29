using Microsoft.EntityFrameworkCore;
using ProServi.Infrastructure.Data;

namespace ProServi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ProServiDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ProServiDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
