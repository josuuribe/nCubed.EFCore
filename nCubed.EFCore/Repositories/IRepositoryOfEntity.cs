using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace nCubed.EFCore.Repositories
{
    public interface IRepository<TEntity> : IRepository, IDisposable
        where TEntity : class
    {

    }
}
