using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace nCubed.EFCore.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Unit of work to use with this Repository.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
