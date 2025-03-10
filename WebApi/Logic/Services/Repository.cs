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

        // Получение всех записей
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Получение записи по id
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Добавление новой записи
        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Обновление существующей записи
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Удаление записи по id
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Запись с id {id} не найдена.");
            }
        }
    }
}
