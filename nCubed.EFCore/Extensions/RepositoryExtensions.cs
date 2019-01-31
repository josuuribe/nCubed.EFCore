using nCubed.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace nCubed.EFCore.Extensions
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Gets an entity set.
        /// </summary>
        /// <typeparam name="TEntity">Entity set type.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <returns>A IQueryable result</returns>
        public static IQueryable<TEntity> Set<TEntity>(this IRepository<TEntity> repository) where TEntity : class
        {
            return repository.UnitOfWork.Set<TEntity>();
        }
        /// <summary>
        /// Get information paged.
        /// </summary>
        /// <typeparam name="TEntity">Entity set type.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="pageIndex">Page index number.</param>
        /// <param name="pageCount">Records number to get.</param>
        /// <param name="orderBy">Property name to order by.</param>
        /// <param name="ascending">True for ascending, otherwise false.</param>
        /// <returns>A IQueryable result</returns>
        public static IQueryable<TEntity> GetPaged<TEntity>(this IRepository<TEntity> repository, int pageIndex, int pageCount, string orderBy, bool ascending = true)
            where TEntity : class
        {
            if (!orderBy.Contains("ascending") && !orderBy.Contains("descending"))
            {
                orderBy = orderBy + " ascending";
            }

            return repository.UnitOfWork.Set<TEntity>().OrderBy(orderBy)
                        .Skip(pageCount * pageIndex)
                        .Take(pageCount);
        }
        /// <summary>
        /// Get information paged.
        /// </summary>
        /// <typeparam name="TEntity">Entity set type.</typeparam>
        /// <typeparam name="KProperty">Property that contains the sort order.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="pageIndex">Page index number.</param>
        /// <param name="pageCount">Records number to get.</param>
        /// <param name="orderByExpression">Property name to order by.</param>
        /// <param name="ascending">True for ascending, otherwise false.</param>
        /// <returns>A IQueryable result</returns>
        public static IQueryable<TEntity> GetPaged<TEntity, KProperty>(this IRepository<TEntity> repository, int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending = true)
                        where TEntity : class
        {
            if (ascending)
            {
                return repository.UnitOfWork.Set<TEntity>().OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return repository.UnitOfWork.Set<TEntity>().OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }
        /// <summary>
        /// Adds a bunch of entities.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to add.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entities">Entities to add.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Add<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities) where TEntity : class
        {
            repository.UnitOfWork.Set<TEntity>().AddRange(entities);
            return repository;
        }
        /// <summary>
        /// Adds a bunch of entities.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to add.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entities">Entities to add.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Add<TEntity>(this IRepository<TEntity> repository, params TEntity[] entities) where TEntity : class
        {
            repository.UnitOfWork.Set<TEntity>().AddRange(entities);
            return repository;
        }
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to add.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity to add.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Add<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            repository.UnitOfWork.Set<TEntity>().Add(entity);
            return repository;
        }
        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to remove.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity to remove.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Delete<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            repository.UnitOfWork.Set<TEntity>().Remove(entity);
            return repository;
        }
        /// <summary>
        /// Deletes a bunch of entities.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to remove.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entities">Entities to add.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Delete<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                repository.UnitOfWork.Set<TEntity>().Remove(entity);
            }
            return repository;
        }
        /// <summary>
        /// Deletes a bunch of entities.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to remove.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entities">Entities to add.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Delete<TEntity>(this IRepository<TEntity> repository, params TEntity[] entities) where TEntity : class
        {
            repository.UnitOfWork.Set<TEntity>().RemoveRange(entities);
            return repository;
        }
        /// <summary>
        /// Finds an entity given given their ids.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to mark.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="ids">Primay keys values, it must be given in the same order as configured in database.</param>
        /// <returns>The repository used for this operation.</returns>
        public static TEntity Find<TEntity>(this IRepository<TEntity> repository, params object[] ids) where TEntity : class
        {
            if (ids.Length == 0)
            {
                throw new ArgumentException($"{nameof(ids)} should not be empty");
            }
            return repository.UnitOfWork.Set<TEntity>().Find(ids);
        }
        /// <summary>
        /// Checks if the given entity id exists in database given their ids.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to mark.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="ids">Primay keys values, it must be given in the same order as configured in database.</param>
        /// <returns>The repository used for this operation.</returns>
        public static bool Exists<TEntity>(this IRepository<TEntity> repository, params object[] ids) where TEntity : class
        {
            return repository.UnitOfWork.Set<TEntity>().Find(ids) != null;
        }
    }
}
