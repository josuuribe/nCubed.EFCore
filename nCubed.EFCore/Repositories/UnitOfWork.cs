using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;

namespace nCubed.EFCore.Repositories
{
    public class UnitOfWork : DbContext, IUnitOfWork
    {
        //        public DbSet<TEntity> CreateSet<TEntity>()
        //where TEntity : class
        //        {
        //            return base.Set<TEntity>();
        //        }

        //    public override EntityEntry<TEntity> Attach<TEntity>(TEntity item)
        //where TEntity : class
        //    {
        //        //attach and set as unchanged
        //        base.Entry<TEntity>(item).State = EntityState.Unchanged;
        //        return new EntityEntry<TEntity>(item);
        //    }

        //    public void SetModified<TEntity>(TEntity item)
        //where TEntity : class
        //    {
        //        //this operation also attach item in object state manager
        //        base.Entry<TEntity>(item).State = EntityState.Modified;
        //    }

        //    public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
        //where TEntity : class
        //    {
        //        //if it is not attached, attach original and set current values
        //        base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        //    }

        public UnitOfWork() : base()
        {

        }

        public UnitOfWork(DbContextOptions options) : base(options)
        {

        }

        public void Commit()
        {
            base.SaveChangesAsync();
        }

        public IEnumerable Local()
        {
            return ChangeTracker.Entries().Select(x => x.Entity);
        }

        public IEnumerable<TEntity> Local<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>().Local;
        }

        public IQueryable<TEntity> LocalOrDatabase<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            var localResults = Local<TEntity>().Where(expression.Compile());
            if (localResults.Any())
            {
                return localResults.AsQueryable();
            }
            return this.Set<TEntity>().Where(expression);
        }

        public void Rollback()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                this.Entry(entry.Entity).CurrentValues.SetValues(this.Entry(entry.Entity).OriginalValues);
                this.Entry(entry.Entity).State = EntityState.Unchanged;
            }
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            // TODO: Use Dapper
            //return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
            return null;
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            // TODO: Use Dapper
            //return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
            return 0;
        }

        public void Reset()
        {
            while (ChangeTracker.Entries().Count() > 0)
            {

                var entry = ChangeTracker.Entries().First();
                var originalValues = this.Entry(entry.Entity).OriginalValues;
                this.Entry(entry.Entity).CurrentValues.SetValues(originalValues);
                entry.State = EntityState.Detached;
            }
        }

        public TContext Context<TContext>() where TContext : DbContext
        {
            return this as TContext;
        }
    }
}
