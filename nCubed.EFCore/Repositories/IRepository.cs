using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Repositories
{
    public interface IRepository
    {
        /// <summary>
        /// Unit of work to use with this Repository.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
