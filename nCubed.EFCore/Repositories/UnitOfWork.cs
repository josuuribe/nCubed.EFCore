using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace nCubed.EFCore.Repositories
{
    public class UnitOfWork : DbContext, IUnitOfWork
    {
        public UnitOfWork() : base()
        {

        }

        public UnitOfWork(DbContextOptions options) : base(options)
        {

        }

        public virtual Task CommitAsync()
        {
            return this.SaveChangesAsync();
        }

        public virtual void Commit()
        {
            this.SaveChanges();
        }

        public virtual IEnumerable Local()
        {
            return ChangeTracker.Entries().Select(x => x.Entity);
        }

        public virtual IEnumerable<TEntity> Local<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>().Local;
        }

        public virtual IQueryable<TEntity> LocalOrDatabase<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            var localResults = Local<TEntity>().Where(expression.Compile());
            if (localResults.Any())
            {
                return localResults.AsQueryable();
            }
            return this.Set<TEntity>().Where(expression);
        }

        public virtual void Rollback()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                this.Entry(entry.Entity).CurrentValues.SetValues(this.Entry(entry.Entity).OriginalValues);
                this.Entry(entry.Entity).State = EntityState.Unchanged;
            }
        }

        public virtual IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery) where TEntity : class, new()
        {
            List<TEntity> entities = new List<TEntity>();
            try
            {
                using (var query = this.Database.GetDbConnection().CreateCommand())
                {
                    query.CommandText = sqlQuery;
                    this.Database.OpenConnection();
                    using (var result = query.ExecuteReader())
                    {
                        var properties = typeof(TEntity).GetProperties();
                        while (result.Read())
                        {
                            TEntity entity = new TEntity();
                            var values = new object[result.FieldCount];
                            result.GetValues(values);
                            for (int i = 0; i < result.FieldCount; i++)
                            {
                                var columnName = result.GetName(i);
                                var property = properties.FirstOrDefault(p => p.Name.ToLower() == columnName.ToLower());
                                if (property != null && !(values[i] is System.DBNull))
                                    property.SetValue(entity, values[i]);
                            }
                            entities.Add(entity);
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException aore)
            {
                throw new ArgumentException($"Wrong Sql Query, some fields not found {sqlQuery}", aore);
            }
            finally
            {
                this.Database.CloseConnection();
            }
            return entities;
        }

        public virtual int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return this.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public virtual void Reset()
        {
            while (ChangeTracker.Entries().Count() > 0)
            {

                var entry = ChangeTracker.Entries().First();
                var originalValues = this.Entry(entry.Entity).OriginalValues;
                this.Entry(entry.Entity).CurrentValues.SetValues(originalValues);
                entry.State = EntityState.Detached;
            }
        }
    }
}
