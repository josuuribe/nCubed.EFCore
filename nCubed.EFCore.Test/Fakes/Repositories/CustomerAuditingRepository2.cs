using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Fakes.Repositories
{
    public class CustomerAuditingRepository2 : IRepository<Customer>
    {
        private readonly ProjectsAuditingContext2 projectsContext = null;
        public IUnitOfWork UnitOfWork => projectsContext;

        public CustomerAuditingRepository2(IUnitOfWork unitOfWork)
        {
            this.projectsContext = unitOfWork as ProjectsAuditingContext2 ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
