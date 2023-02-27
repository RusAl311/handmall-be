using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected DatabaseContext _dbContext;
    public BaseRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> Add(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return true;
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }
}