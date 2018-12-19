using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Repositories
{
    public interface IEFUnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
        int ExecuteCommand(string sqlCommand, params object[] parameters);
    }
}
