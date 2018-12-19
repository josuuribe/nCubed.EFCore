using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GFI.Common.EFCore
{
    public static class DbContextExtensions
    {
        public static IEnumerable<IKey> GetEntityKey(this DbContext context, object entity)
        {
            var entry = context.Entry(entity);
            var keyNames = context.Model.FindEntityType(entity.GetType()).FindPrimaryKey().Properties.Select(x => x.Name);
            var keys = entry.Metadata.GetKeys();
            return keys;
        }

        //public static IEnumerable<EntityEntry<T>> Local<T>(this DbContext context) where T : class
        //{
        //    return context.ChangeTracker.Entries<T>();
        //}

        //public static IEnumerable<EntityEntry<T>> Local<T>(this DbSet<T> set) where T : class
        //{
        //    if (set is InternalDbSet<T>)
        //    {
        //        var svcs = (set as InternalDbSet<T>).GetInfrastructure().GetService<IDbContextServices>();
        //        var ctx = svcs.CurrentContext.Context;
        //        return Local<T>(ctx);
        //    }
        //    throw new ArgumentException("Invalid set", "set");
        //}

        //public static IQueryable<T> LocalOrDatabase<T>(this DbContext context, Expression<Func<T, bool>> expression) where T : class
        //{
        //    var localResults = context.Set<T>().Local().Where(expression.Compile());
        //    if (localResults.Any()) {
        //        return localResults.AsQueryable();
        //    }
        //    return context.Set<T>().Where(expression);
        //}
    }
}
