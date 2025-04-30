using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Repositories.Interfaces;

namespace WebApi.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CoffeeSharpDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CoffeeSharpDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        private IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? filter = null,
            List<Expression<Func<T, object>>>? includes = null,
            bool disableTracking = false)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            includes?.ForEach(include => query = query.Include(include));

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public async Task<int> CountAsync(
           Expression<Func<T, bool>>? predicate = null,
           CancellationToken cancellationToken = default)
        {
            return predicate == null
                ? await _dbSet.CountAsync(cancellationToken)
                : await _dbSet.CountAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<Expression<Func<T, object>>>? includes = null,
            bool disableTracking = false,
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filter, includes, disableTracking);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageSize != int.MaxValue)
            {
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            List<Expression<Func<T, object>>>? includes = null,
            bool disableTracking = false,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filter, includes, disableTracking);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<T> AddOneAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity, cancellationToken);

            return entity;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var entityList = entities.ToList();

            if (entityList.Count == 0)
            {
                return entityList;
            }

            await _dbSet.AddRangeAsync(entityList, cancellationToken);

            return entityList;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Update(entity);

            return entity;
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            _dbSet.Remove(entity);
        }
    }
}
