using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Fakes.Repositories
{
    public class CustomerAuditingRepository3 : IRepository<Customer>
    {
        private readonly ProjectsNullAuditingContext projectsContext = null;
        public IUnitOfWork UnitOfWork => projectsContext;

        public CustomerAuditingRepository3(IUnitOfWork unitOfWork)
        {
            this.projectsContext = unitOfWork as ProjectsNullAuditingContext ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
