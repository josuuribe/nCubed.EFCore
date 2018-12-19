using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nCubed.EFCore.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
        IEnumerable<TEntity> Local<TEntity>() where TEntity : class;
        IEnumerable Local();
        void Reset();
        //DbSet<TEntity> Set<TEntity>() where TEntity : class;
        //EntityEntry<TEntity> Attach<TEntity>(TEntity item) where TEntity : class;
        //void SetModified<TEntity>(TEntity item) where TEntity : class;
        //void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class;
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        TContext Context<TContext>() where TContext : DbContext;
    }
}
