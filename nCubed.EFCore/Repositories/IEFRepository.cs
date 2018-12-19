using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Repositories
{
    public interface IEFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        IEFUnitOfWork<TContext> Using<TContext>() where TContext : DbContext;
    }
}
