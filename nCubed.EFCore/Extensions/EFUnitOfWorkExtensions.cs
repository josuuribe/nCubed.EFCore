using nCubed.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nCubed.EFCore.Extensions
{
    public static class EFUnitOfWorkExtensions
    {
        private static DbContext CheckDbContext(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is DbContext context))
                throw new ArgumentException("Parameter is not a DbContext.");
            return context;
        }
        /// <summary>
        /// Returns all entities tracked.
        /// </summary>
        /// <param name="unitOfWork">IUnitOfWork that holds all entities.</param>
        /// <returns>IEnumerable containing all tracked entities.</returns>
        public static IEnumerable GetTrackedEntities(this IUnitOfWork unitOfWork)
        {
            return CheckDbContext(unitOfWork).ChangeTracker.Entries().Select(o => o.Entity);
        }
        /// <summary>
        /// Returns all entities tracked by this UnitOfWork filtered by type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to get.</typeparam>
        /// <param name="unitOfWork">IUnitOfWork that holds all entities.</param>
        /// <returns>IEnumerable containing all tracked entities filtered by type.</returns>
        public static IEnumerable<TEntity> GetTrackedEntities<TEntity>(this IUnitOfWork unitOfWork) where TEntity : class
        {
            return CheckDbContext(unitOfWork).ChangeTracker.Entries<TEntity>().Select(o => o.Entity);
        }
        /// <summary>
        /// Gets all entities marked as modified.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to get.</typeparam>
        /// <param name="unitOfWork">IUnitOfWork that holds all entities.</param>
        /// <returns>IEnumerable containing all entities marked as modified.</returns>
        public static IEnumerable<TEntity> GetModified<TEntity>(this IUnitOfWork unitOfWork) where TEntity : class
        {            
            return CheckDbContext(unitOfWork).ChangeTracker.Entries<TEntity>().Where(e => e.State == EntityState.Modified).Select(o => o.Entity);
        }
        /// <summary>
        /// Gets all entities marked as added.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to get.</typeparam>
        /// <param name="unitOfWork">IUnitOfWork that holds all entities.</param>
        /// <returns>IEnumerable containing all entities marked as added.</returns>
        public static IEnumerable<TEntity> GetAdded<TEntity>(this IUnitOfWork unitOfWork) where TEntity : class
        {
            return CheckDbContext(unitOfWork).ChangeTracker.Entries<TEntity>().Where(e => e.State == EntityState.Added).Select(o => o.Entity);
        }
        /// <summary>
        /// Gets all entities marked as deleted.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to get.</typeparam>
        /// <param name="unitOfWork">IUnitOfWork that holds all entities.</param>
        /// <returns>IEnumerable containing all entities marked as deleted.</returns>
        public static IEnumerable<TEntity> GetDeleted<TEntity>(this IUnitOfWork unitOfWork) where TEntity : class
        {
            return CheckDbContext(unitOfWork).ChangeTracker.Entries<TEntity>().Where(e => e.State == EntityState.Deleted).Select(o => o.Entity);
        }
        /// <summary>
        /// Gets the EF entry related to this entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to attach.</typeparam>
        /// <param name="unitOfWork">UnitOfWork that contains all entities.</param>
        /// <param name="entity">Entity that will be attached.</param>
        /// <returns>UnitOfWork used for this operation.</returns>
        public static EntityEntry<TEntity> Entry<TEntity>(this IUnitOfWork unitOfWork, TEntity entity) where TEntity : class
        {
            return CheckDbContext(unitOfWork).ChangeTracker.Context.Entry<TEntity>(entity);
        }
        /// <summary>
        /// Attachs an entity, the entity will be tracked.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to attach.</typeparam>
        /// <param name="unitOfWork">UnitOfWork that contains all entities.</param>
        /// <param name="entity">Entity that will be attached.</param>
        /// <returns>UnitOfWork used for this operation.</returns>
        public static IUnitOfWork Attach<TEntity>(this IUnitOfWork unitOfWork, TEntity entity) where TEntity : class
        {
            CheckDbContext(unitOfWork).ChangeTracker.Context.Attach<TEntity>(entity);
            return unitOfWork;
        }
        /// <summary>
        /// Attachs an entity and mark it as modified, so this entity will be persisted in database as is.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to mark.</typeparam>
        /// <param name="unitOfWork">UnitOfWork that contains all entities.</param>
        /// <param name="entity">Entity that will be marked.</param>
        /// <returns>UnitOfWork used for this operation.</returns>
        public static IUnitOfWork Update<TEntity>(this IUnitOfWork unitOfWork, TEntity entity) 
            where TEntity : class
        {
            CheckDbContext(unitOfWork).ChangeTracker.Context.Update<TEntity>(entity);
            return unitOfWork;
        }
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="unitOfWork">Unit of work that will store this entity.</param>
        /// <param name="entity">Entity to add.</param>
        /// <returns>This UnitOfWork.</returns>
        public static IUnitOfWork Add(this IUnitOfWork unitOfWork, object entity) 
        {
            CheckDbContext(unitOfWork).Add(entity);
            return unitOfWork;
        }
        /// <summary>
        /// Adds a bunch of entities.
        /// </summary>
        /// <param name="unitOfWork">Unit of work that will store these entities.</param>
        /// <param name="entity">Entities to add.</param>
        /// <returns>This UnitOfWork.</returns>
        public static IUnitOfWork Add(this IUnitOfWork unitOfWork, object[] entities)
        {
            CheckDbContext(unitOfWork).AddRange(entities);
            return unitOfWork;
        }
        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="unitOfWork">Unit of work that will store this entity.</param>
        /// <param name="entity">Entity to remove.</param>
        /// <returns>This UnitOfWork.</returns>
        public static IUnitOfWork Remove(this IUnitOfWork unitOfWork, object entity)
        {
            CheckDbContext(unitOfWork).Remove(entity);
            return unitOfWork;
        }
        /// <summary>
        /// Removes a bunch of entities.
        /// </summary>
        /// <param name="unitOfWork">Unit of work that will remove these entities.</param>
        /// <param name="entities">Entities to remove.</param>
        /// <returns>This UnitOfWork.</returns>
        public static IUnitOfWork Remove(this IUnitOfWork unitOfWork, object[] entities)
        {
            CheckDbContext(unitOfWork).Remove(entities);
            return unitOfWork;
        }
    }
}
