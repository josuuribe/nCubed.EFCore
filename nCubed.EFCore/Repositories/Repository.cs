using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace nCubed.EFCore.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext dbContext;
        public IUnitOfWork UnitOfWork
        {
            get;private set;
        }

        //public DbSet<TEntity> Set
        //{
        //    get
        //    {
        //        return UnitOfWork.Set<TEntity>();
        //    }
        //}

        public Repository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this.dbContext = unitOfWork as DbContext;
            this.UnitOfWork = unitOfWork;
        }

        //public virtual void Add(TEntity item)
        //{

        //    if (item != (TEntity)null)
        //        GetSet().Add(item); // add new item in this set
        //    else
        //    {
        //        /*
        //        LoggerFactory.CreateLog()
        //                  .LogInfo(Messages.info_CannotAddNullEntity, typeof(TEntity).ToString());
        //                  */
        //    }
        //}

        //public virtual void Remove(TEntity item)
        //{
        //    if (item != (TEntity)null)
        //    {
        //        //attach item if not exist
        //        //UnitOfWork.Attach(item);

        //        //set as "removed"
        //        GetSet().Remove(item);
        //    }
        //    else
        //    {
        //        /*
        //        LoggerFactory.CreateLog()
        //                  .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
        //                  */
        //    }
        //}

        //public virtual void TrackItem(TEntity item)
        //{
        //    if (item != (TEntity)null)
        //    { }
        //        //UnitOfWork.Attach<TEntity>(item);
        //    else
        //    {
        //        /*
        //        LoggerFactory.CreateLog()
        //                  .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
        //                  */
        //    }
        //}

        //public virtual void Modify(TEntity item)
        //{
        //    if (item != (TEntity)null)
        //        UnitOfWork.SetModified(item);
        //    else
        //    {
        //        /*
        //        LoggerFactory.CreateLog()
        //                  .LogInfo(Messages.info_CannotModifyNullEntity, typeof(TEntity).ToString());
        //                  */
        //    }
        //}

        //public virtual TEntity Get(Guid id)
        //{
        //    if (id != Guid.Empty)
        //        return GetSet().Find(id);
        //    else
        //        return null;
        //}

        //public virtual IEnumerable<TEntity> GetAll()
        //{
        //    return GetSet();
        //}

        //public virtual void Merge(TEntity persisted, TEntity current)
        //{
        //    UnitOfWork.ApplyCurrentValues(persisted, current);
        //}

        //private DbSet<TEntity> GetSet()
        //{
        //    return UnitOfWork.CreateSet<TEntity>();
        //}

        //public virtual bool Exists(Guid id)
        //{
        //    return UnitOfWork.CreateSet<TEntity>().Find(id) != null;
        //}

        //public virtual bool Exists(int id)
        //{
        //    return UnitOfWork.CreateSet<TEntity>().Find(id) != null;
        //}

        //public virtual bool Exists(string id)
        //{
        //    return UnitOfWork.CreateSet<TEntity>().Find(id) != null;
        //}

        //public virtual IQueryable<TEntity> GetPaged(int pageIndex, int pageCount, string orderBy)
        //{
        //    var set = GetSet();

        //    return set.Skip(pageCount * pageIndex).Take(pageCount).OrderBy(orderBy);
        //}

        //public virtual IQueryable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending = false)
        //{
        //    var set = GetSet();

        //    if (ascending)
        //    {
        //        return set.OrderBy(orderByExpression)
        //                  .Skip(pageCount * pageIndex)
        //                  .Take(pageCount);
        //    }
        //    else
        //    {
        //        return set.OrderByDescending(orderByExpression)
        //                  .Skip(pageCount * pageIndex)
        //                  .Take(pageCount);
        //    }
        //}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    UnitOfWork.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Repository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
