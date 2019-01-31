using Microsoft.EntityFrameworkCore;
using nCubed.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Extensions
{
    public static class EFRepositoryExtensions
    {
        /// <summary>
        /// Merges all values from one entity into another entity, all properties that exists in both will be overriten by new entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to merge.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity that will be overrwritten.</param>
        /// <param name="newValues">Entity taht supplies new values.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Merge<TEntity>(this IRepository<TEntity> repository, TEntity entity, TEntity newValues) where TEntity : class
        {
            repository.UnitOfWork.Entry(entity).CurrentValues.SetValues(newValues);
            return repository;
        }
        /// <summary>
        /// Sets all values to original entity values (the ones from database).
        /// </summary>
        /// <typeparam name="TEntity">Entity type to reset.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity that will be overrwritten.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Reset<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            //var repository.UnitOfWork = UnitOfWorkAsDbrepository.UnitOfWork(repository);

            repository.UnitOfWork.Entry(entity).CurrentValues.SetValues(repository.UnitOfWork.Entry<TEntity>(entity).OriginalValues);
            repository.UnitOfWork.Entry(entity).State = EntityState.Unchanged;
            return repository;
        }
        /// <summary>
        /// Reloads the entity, updating this entity from database.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to update.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity that will be updated.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Refresh<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            repository.UnitOfWork.Entry(entity).Reload();
            repository.UnitOfWork.Entry(entity).State = EntityState.Unchanged;
            entity = repository.UnitOfWork.Entry(entity).Entity;
            return repository;
        }
        /// <summary>
        /// Creates an entity using the original values in database for this particular entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to get database information.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity that will be used to get information.</param>
        /// <returns>The repository used for this operation.</returns>
        public static TEntity Source<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            return repository.UnitOfWork.Entry(entity).GetDatabaseValues().ToObject() as TEntity;
        }
        /// <summary>
        /// Detachs this entity, this entity will not be tracked.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to detach.</typeparam>
        /// <param name="repository">Repository to use for.</param>
        /// <param name="entity">Entity that will be detached.</param>
        /// <returns>The repository used for this operation.</returns>
        public static IRepository<TEntity> Detach<TEntity>(this IRepository<TEntity> repository, TEntity entity) where TEntity : class
        {
            repository.UnitOfWork.Entry(entity).State = EntityState.Detached;
            return repository;
        }
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="repository">Repository to use.</param>
        /// <param name="entity">Entity to add.</param>
        /// <returns>This repository.</returns>
        public static IRepository Add<TEntity>(this IRepository repository, object entity) where TEntity : class
        {
            repository.UnitOfWork.Add(entity);
            return repository;
        }
        /// <summary>
        /// Adds a bunch of entities.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to add.</typeparam>
        /// <param name="repository">Repository to use.</param>
        /// <param name="entities">Entities to add.</param>
        /// <returns>This repository.</returns>
        public static IRepository Add<TEntity>(this IRepository repository, params object[] entities) where TEntity : class
        {
            repository.UnitOfWork.Add(entities);
            return repository;
        }
        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="repository">Repository to use.</param>
        /// <param name="entity">Entity to add.</param>
        /// <returns>This repository.</returns>
        public static IRepository Remove<TEntity>(this IRepository repository, object entity) where TEntity : class
        {
            repository.UnitOfWork.Remove(entity);
            return repository;
        }
        /// <summary>
        /// Removes a bunch of entities.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to add.</typeparam>
        /// <param name="entity">Entity to add.</param>
        /// <param name="entities">Entities to add.</param>
        /// <returns>This repository.</returns>
        public static IRepository Remove<TEntity>(this IRepository repository, object[] entities) where TEntity : class
        {
            repository.UnitOfWork.Remove(entities);
            return repository;

        }
    }
}