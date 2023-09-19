using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly AppContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(AppContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T> GetById(int id)
    {
        var entity = await _dbSet.FindAsync(id);

        return entity;
    }

    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public async Task<Result<bool>> Add(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            
            await _context.SaveChangesAsync();
            
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, $"Failed to add the entity in data base. Error: {e.Message}");
        }
    }

    public async Task<Result<bool>> Delete(T entity)
    {
        try
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            
            _dbSet.Remove(entity);
            
            await _context.SaveChangesAsync();
            
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, $"Failed to delete the entity in data base. Error: {e.Message}");
        }
    }

    public async Task<Result<bool>> Update(T entity)
    {
        try
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, $"Failed to update the entity in data base. Error: {e.Message}");
        }
    }
}