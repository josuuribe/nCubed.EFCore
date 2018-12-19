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
        IUnitOfWork UnitOfWork { get; }
        //void Add(TEntity item);
        //void Remove(TEntity item);
        //void Modify(TEntity item);
        //void TrackItem(TEntity item);
        //void Merge(TEntity persisted, TEntity current);
        //bool Exists(Guid id);
        //bool Exists(int id);
        //bool Exists(string id);
        //IQueryable<TEntity> GetPaged(int pageIndex, int pageCount, string orderBy);
        //IQueryable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending = false);
        //TEntity Get(Guid id);
        //IEnumerable<TEntity> GetAll();
        //DbSet<TEntity> Set { get; }
        //void SetModified(TEntity item);
        //void ApplyCurrentValues(TEntity original, TEntity current);
    }
}
