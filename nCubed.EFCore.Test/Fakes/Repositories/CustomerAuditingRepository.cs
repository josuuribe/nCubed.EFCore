using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Fakes.Repositories
{
    public class CustomerAuditingRepository : IRepository<Customer>
    {
        private readonly ProjectsAuditingContext projectsContext = null;
        public IUnitOfWork UnitOfWork => projectsContext;

        public CustomerAuditingRepository(IUnitOfWork unitOfWork)
        {
            this.projectsContext = unitOfWork as ProjectsAuditingContext ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
