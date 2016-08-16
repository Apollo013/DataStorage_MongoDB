using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace DataAccessLayer.Extensions
{
    public static class QueryableExtenstions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string orderBy)
        {
            return query.OrderBy<T>(orderBy);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> list, Expression<Func<T, object>> orderBy, bool isDescending)
        {
            return isDescending ? list.OrderByDescending(orderBy) : list.OrderBy(orderBy);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> list, int pageNumber, int pageSize)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
