using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace DataAccessLayer.Extensions
{
    /// <summary>
    /// Extension class for IEnumerable<T>
    /// </summary>
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> list, string orderBy)
        {
            return list.AsQueryable().OrderBy(orderBy);
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> list, Expression<Func<T, object>> orderBy, bool isDescending)
        {
            return isDescending ? list.AsQueryable().OrderByDescending(orderBy) : list.AsQueryable().OrderBy(orderBy);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> list, int pageNumber, int pageSize)
        {
            return list.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
