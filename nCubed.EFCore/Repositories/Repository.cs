namespace nCubed.EFCore.Repositories
{
    internal class Repository : IRepository
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; private set; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
