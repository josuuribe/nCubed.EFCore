using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Fakes;
using nCubed.EFCore.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Repositories.Fakes
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ProjectsContext projectsContext = null;
        public IUnitOfWork UnitOfWork => projectsContext;

        public CustomerRepository(IUnitOfWork unitOfWork)
        {
            this.projectsContext = unitOfWork as ProjectsContext ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
