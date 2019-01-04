using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nCubed.EFCore.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Apply all changes in datasource.
        /// </summary>
        void Commit();
        /// <summary>
        /// Rollback all changes, set values to original values (if the entity comes from database) and set it as no changed but is still tracked.
        /// </summary>
        void Rollback();
        /// <summary>
        /// Get local entities, the ones that are being used but not yet saved to database.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to be get.</typeparam>
        /// <returns>All entities being currently tracked.</returns>
        IEnumerable<TEntity> Local<TEntity>() where TEntity : class;
        /// <summary>
        /// Get local entities, the ones that are being used but not yet saved to database.
        /// </summary>
        /// <returns>All entities being currently tracked.</returns>
        IEnumerable Local();
        /// <summary>
        /// Executes an expression to get values from local database, if query returns empty the results are get from database.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="expression">Expression that returns entities.</param>
        /// <returns>A Queryable entity list.</returns>
        IQueryable<TEntity> LocalOrDatabase<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        /// <summary>
        /// Clear all changes for all tracked entities and removes tracking.
        /// </summary>
        void Reset();
        /// <summary>
        /// Get all entities that have been modified and are going to be updated in database.
        /// </summary>
        /// <returns>All entities to be updated.</returns>
        IEnumerable GetModifiedEntities();
        /// <summary>
        /// Get all entities that have been added and are going to be inserted in database.
        /// </summary>
        /// <returns>All entities to be added.</returns>
        IEnumerable GetAddedEntities();
        /// <summary>
        /// Get all entities that have been deleted and are going to be removed from database.
        /// </summary>
        /// <returns>All entities to be added.</returns>
        IEnumerable GetDeletedEntities();
        /// <summary>
        /// Get entity set for given type.
        /// </summary>
        /// <typeparam name="TEntity">Entity set type.</typeparam>
        /// <returns>A DbSet with all entities in database.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        /// <summary>
        /// Executes a query, usually a SELECT statement.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to be used to create objects.</typeparam>
        /// <param name="sqlQuery">SELECT statement to execute, there are no extra validations on parameters.</param>
        /// <returns>The SELECT result casted to Entity type.</returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery) where TEntity : class, new();
        /// <summary>
        /// Executes a command, like INSERT, store procedure...
        /// </summary>
        /// <param name="sqlCommand">Command to execute.</param>
        /// <param name="parameters">Parameters to use in command.</param>
        /// <returns>int value with all affected rows.</returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        /// <summary>
        /// The context in use, useful for extra actions not covered by actual API version.
        /// </summary>
        /// <typeparam name="TContext">Context type.</typeparam>
        /// <returns>The context.</returns>
        TContext Context<TContext>() where TContext : DbContext;
    }
}
