using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Fakes;
using nCubed.EFCore.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Repositories.Fakes
{
    public class TechnologyRepository : IRepository<Technology>
    {
        private ProjectsContext projectsContext;
        public IUnitOfWork UnitOfWork => projectsContext;


        public TechnologyRepository(IUnitOfWork unitOfWork)
        {
            this.projectsContext = unitOfWork as ProjectsContext ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
