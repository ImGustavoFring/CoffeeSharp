using System.Linq.Expressions;

namespace WebApi.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default);
        Task<T> AddOneAsync(T entity,
            CancellationToken cancellationToken = default);
        Task<int> CountAsync(
           Expression<Func<T, bool>>? predicate = null,
           CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<Expression<Func<T, object>>>? includes = null,
            bool disableTracking = false,
            int pageIndex = 0, int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> filter,
            List<Expression<Func<T, object>>>? includes = null,
            bool disableTracking = false,
            CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(object id,
            CancellationToken cancellationToken = default);
        T Update(T entity);
        Task DeleteAsync(object id,
            CancellationToken cancellationToken = default);
    }
}