using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace nCubed.EFCore.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// SQL "between" implementation, it adds a between constraint to existing query.
        /// </summary>
        /// <typeparam name="TSource">Entity to use.</typeparam>
        /// <typeparam name="TKey">Between property to use.</typeparam>
        /// <param name="source">Query with original data.</param>
        /// <param name="keySelector">Property to be used in between command.</param>
        /// <param name="low">Minimun value.</param>
        /// <param name="high">Maximun value.</param>
        /// <returns></returns>
        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, TKey low, TKey high) where TKey : IComparable<TKey>
        {
            var sourceParameter = Expression.Parameter(typeof(TSource));
            var body = keySelector.Body;
            var parameter = keySelector.Parameters[0];
            var compareMethod = typeof(TKey).GetMethod("CompareTo", new Type[] { typeof(TKey) });
            var zero = Expression.Constant(0, typeof(int));
            var upper = Expression.LessThanOrEqual(Expression.Call(body, compareMethod, Expression.Constant(high)), zero);
            var lower = Expression.GreaterThanOrEqual(Expression.Call(body, compareMethod, Expression.Constant(low)), zero);
            var andExpression = Expression.AndAlso(upper, lower);
            var whereCallExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Lambda<Func<TSource, Boolean>>(andExpression, new ParameterExpression[] { parameter }));
            return source.Provider.CreateQuery<TSource>(whereCallExpression);
        }
        /// <summary>
        /// Returns distinct elements based on property
        /// </summary>
        /// <typeparam name="TSource">Entity to use.</typeparam>
        /// <typeparam name="TKey">Distinct property to use.</typeparam>
        /// <param name="source">Query with original data.</param>
        /// <param name="keySelector"></param>
        /// <returns>Filtered list</returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var list = source.ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                var from = list[i];

                for (int j = i + 1; j < list.Count(); j++)
                {
                    var to = list[j];
                    if (keySelector(from).Equals(keySelector(to)))
                    {
                        list.Remove(to);
                    }
                }
                yield return from;
            }
        }
    }
}
