using nCubed.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;

namespace nCubed.EFCore.Extensions
{
    public static class RepositoryExtensions
    {
        private static DbContext UnitOfWorkAsDbContext<TEntity>(IRepository<TEntity> repository) where TEntity : class
        {
            if (repository.UnitOfWork == null)
                throw new ArgumentNullException(nameof(repository.UnitOfWork));
            if (!(repository.UnitOfWork is DbContext context))
                throw new ArgumentException(nameof(repository.UnitOfWork));
            return context;
        }

        public static IQueryable<TEntity> Set<TEntity>(this IRepository<TEntity> repository) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            return context.Set<TEntity>();
        }

        public static IQueryable<TEntity> GetPaged<TEntity>(this IRepository<TEntity> repository, int pageIndex, int pageCount, string orderBy, bool ascending = true)
            where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            if (!orderBy.Contains("ascending") && !orderBy.Contains("descending"))
            {
                orderBy = orderBy + " ascending";
            }

            if (ascending)
            {
                return context.Set<TEntity>().OrderBy(orderBy)
                            .Skip(pageCount * pageIndex)
                            .Take(pageCount);
            }
            else
            {
                return context.Set<TEntity>().OrderBy(orderBy)
                            .Skip(pageCount * pageIndex)
                            .Take(pageCount);
            }
        }

        public static IQueryable<TEntity> GetPaged<TEntity, KProperty>(this IRepository<TEntity> repository, int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending = true)
                        where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            if (ascending)
            {
                return context.Set<TEntity>().OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return context.Set<TEntity>().OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }

        public static IRepository<TEntity> AddRange<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Set<TEntity>().AddRange(entities);

            return repository;
        }

        public static IRepository<TEntity> Add<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Set<TEntity>().Add(entity);

            return repository;
        }

        public static IRepository<TEntity> Delete<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Set<TEntity>().Remove(entity);

            return repository;
        }

        public static IRepository<TEntity> Apply<TEntity>(this IRepository<TEntity> repository, TEntity entity, TEntity newValues) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Entry<TEntity>(entity).CurrentValues.SetValues(newValues);

            return repository;
        }

        public static IRepository<TEntity> Reset<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Entry<TEntity>(entity).CurrentValues.SetValues(context.Entry<TEntity>(entity).OriginalValues);
            context.Entry<TEntity>(entity).State = EntityState.Unchanged;

            return repository;
        }

        public static IRepository<TEntity> Refresh<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Entry<TEntity>(entity).Reload();
            context.Entry<TEntity>(entity).State = EntityState.Unchanged;

            return repository;
        }

        public static TEntity Source<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            return context.Entry<TEntity>(entity).GetDatabaseValues().ToObject() as TEntity;
        }

        public static IRepository<TEntity> Detach<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Entry<TEntity>(entity).State = EntityState.Detached;

            return repository;
        }

        public static IRepository<TEntity> Attach<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Attach<TEntity>(entity);

            return repository;
        }

        public static IRepository<TEntity> MarkAsModified<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);

            context.Update<TEntity>(entity);

            return repository;
        }

        public static TEntity Find<TEntity>(this IRepository<TEntity> repository, TEntity entity, params object[] ids) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);
            return context.Set<TEntity>().Find(ids);
        }

        public static bool Exists<TEntity>(this IRepository<TEntity> repository, TEntity entity, params object[] ids) where TEntity : class
        {
            var context = UnitOfWorkAsDbContext(repository);
            return context.Set<TEntity>().Find(ids) != null;
        }
    }
}
