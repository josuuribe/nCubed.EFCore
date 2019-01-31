using nCubed.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Extensions
{
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Creates a typed virtual repository using specified UnitOfWork for create it.
        /// </summary>
        /// <typeparam name="TEntity">Entity type repository.</typeparam>
        /// <param name="unitOfWork">UnitOfWOrk used for this repository.</param>
        /// <returns>The new repository.</returns>
        public static IRepository<TEntity> Virtual<TEntity>(this IUnitOfWork unitOfWork)
            where TEntity : class
        {
            Repository<TEntity> repository = new Repository<TEntity>(unitOfWork);
            return repository;
        }
        /// <summary>
        /// Creates a non typed virtual repository using specified UnitOfWork.
        /// </summary>
        /// <param name="unitOfWork">UnitOfWOrk used for this repository.</param>
        /// <returns>The new repository.</returns>
        public static IRepository Virtual(IUnitOfWork unitOfWork)
        {
            Repository repository = new Repository(unitOfWork);
            return repository;
        }
    }
}
