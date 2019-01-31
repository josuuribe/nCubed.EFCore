namespace nCubed.EFCore.Repositories
{
    internal class Repository<TEntity> : Repository, IRepository<TEntity> where TEntity : class
    {
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
