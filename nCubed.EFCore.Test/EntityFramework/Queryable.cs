using nCubed.EFCore.Extensions;
using nCubed.EFCore.Test.Fakes;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using nCubed.EFCore.Test.Repositories.Fakes;

namespace nCubed.EFCore.Test.EntityFramework
{
    public class Queryable : Context
    {
        private readonly CustomerRepository customerRepository = null;

        public Queryable()
        {
            customerRepository = new CustomerRepository(new ProjectsContext(dbContextOptions));
        }
    }
}
