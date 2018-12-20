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
using Microsoft.EntityFrameworkCore.Metadata;

namespace nCubed.EFCore.Test.EntityFramework
{
    public class Queryable : Context
    {
        private readonly CustomerRepository customerRepository = null;

        //public TestBetween()
        //{
        //    ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
        //    Customer customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
        //    ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone2", Email = "demo2@demo.es" };
        //    Customer customer2 = new Customer() { Name = "Customer2", ContactInformation = contactInformation2 };

        //    customerRepository = new CustomerRepository(new ProjectsContext(dbContextOptions));
        //    customerRepository.Add(customer1).Add(customer2);

        //    customerRepository.Set().Between<Customer, int>()
        //}
    }
}
