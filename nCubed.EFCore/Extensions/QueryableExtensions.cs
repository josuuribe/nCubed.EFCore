using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace nCubed.EFCore.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DependenciesProperty = typeof(Database).GetTypeInfo().GetDeclaredProperty("Dependencies");


        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> property, TKey low, TKey high) where TKey : IComparable<TKey>
        {
            var sourceParameter = Expression.Parameter(typeof(TSource));
            var body = property.Body;
            var parameter = property.Parameters[0];
            var compareMethod = typeof(TKey).GetMethod("CompareTo", new Type[] { typeof(TKey) });
            var zero = Expression.Constant(0, typeof(int));
            var upper = Expression.LessThanOrEqual(Expression.Call(body, compareMethod, Expression.Constant(high)), zero);
            var lower = Expression.GreaterThanOrEqual(Expression.Call(body, compareMethod, Expression.Constant(low)), zero);
            var andExpression = Expression.AndAlso(upper, lower);
            var whereCallExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Lambda<Func<TSource, Boolean>>(andExpression, new ParameterExpression[] { parameter }));
            return source.Provider.CreateQuery<TSource>(whereCallExpression);
        }

    }
}
