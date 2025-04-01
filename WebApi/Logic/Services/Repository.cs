using CoffeeSharp.WebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CoffeeSharpDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CoffeeSharpDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }
        }
    }
}
