using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace DataAccessLayer.Extensions
{
    public static class FindFluentExtensions
    {
        /// <summary>
        /// Sorts the query according to the specified expression order and direction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query to be sorted</param>
        /// <param name="orderBy">The expression in which to order the results</param>
        /// <param name="isDescending">true if results to be processed in reverse order, false to process in ascending order</param>
        /// <returns></returns>
        public static IFindFluent<T, T> Sort<T>(this IFindFluent<T, T> query, Expression<Func<T, object>> orderBy, bool isDescending)
        {
            return isDescending ? query.SortByDescending(orderBy) : query.SortBy(orderBy);
        }
    }
}
