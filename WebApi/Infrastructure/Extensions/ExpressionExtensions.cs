using System.Linq.Expressions;

namespace WebApi.Infrastructure.Extensions
{
    internal static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(first, param),
                Expression.Invoke(second, param));

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
