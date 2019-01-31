using nCubed.EFCore.Test.Fakes;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using nCubed.EFCore.Extensions;
using FluentAssertions;
using System.Linq;
using nCubed.EFCore.Test.Repositories.Fakes;

namespace nCubed.EFCore.Test.DataAccess
{
    public class UnitOfWork : Context
    {
        ProjectsContext projectsContext = null;
        CustomerRepository customerRepository = null;
        TechnologyRepository technologyRepository = null;
        public UnitOfWork()
        {
            projectsContext = new ProjectsContext(dbContextOptions);
            customerRepository = new CustomerRepository(projectsContext);
            technologyRepository = new TechnologyRepository(projectsContext);
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestTrackedEntitiesByType()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
            ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer2 = new Customer() { Name = "Customer1", ContactInformation = contactInformation2 };
            Technology technology = new Technology();
            technology.Name = "Name";

            customerRepository.Add(customer1);
            customerRepository.Add(customer2);

            technologyRepository.Add(technology);

            var customersTracked = customerRepository.UnitOfWork.Local<Customer>();

            customersTracked.Should().NotBeEmpty()
                    .And.HaveCount(2)
                    .And.ContainItemsAssignableTo<Customer>();
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestTrackedEntities()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
            ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone2", Email = "demo2@demo.es" };
            var customer2 = new Customer() { Name = "Customer2", ContactInformation = contactInformation2 };
            Technology technology = new Technology();
            technology.Name = "Name";

            var context = new ProjectsContext(dbContextOptions);

            customerRepository.Add(customer1);
            customerRepository.Add(customer2);
            technologyRepository.Add(technology);

            var customersTracked = customerRepository.UnitOfWork.Local();

            customersTracked.Should().NotBeEmpty()
                    .And.HaveCount(5);
            customersTracked.AsQueryable().OfType<Customer>().Should().NotBeEmpty()
                    .And.HaveCount(2);
            customersTracked.AsQueryable().OfType<Technology>().Should().NotBeEmpty()
                    .And.HaveCount(1);
            customersTracked.AsQueryable().OfType<ContactInformation>().Should().NotBeEmpty()
                    .And.HaveCount(2);
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestReset()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };


            customerRepository.Add(customer1);
            customerRepository.UnitOfWork.Commit();

            customer1.Name = "Changed name1";
            customerRepository.UnitOfWork.Reset();


            customer1.Name.Should().Be("Customer1", "Name different.");
            customerRepository.UnitOfWork.Local<Customer>().Should().BeEmpty();
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestQuery()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
            ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone2", Email = "demo2@demo.es" };
            var customer2 = new Customer() { Name = "Customer2", ContactInformation = contactInformation2 };

            var customers = new List<Customer>() { customer1, customer2 };

            customerRepository.Add(customers);
            customerRepository.UnitOfWork.Commit();

            var customerRepos = customerRepository.UnitOfWork.ExecuteQuery<Customer>("SELECT CUSTOMER_ID as CustomerId, NAME as Name, PHONE as Phone, EMAIL as email FROM CUSTOMERS");

            customerRepos.Should().NotBeEmpty()
                    .And.HaveCount(2)
                    .And.ContainItemsAssignableTo<Customer>()
                    .And.Equal(customers, (c1, c2) => c1.Name == c2.Name);
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestMarkAsAdded()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository.Add(customer);
            var updates = customerRepository.UnitOfWork.GetAdded<Customer>();

            updates.Should().NotBeEmpty()
                .And.HaveCount(1);
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestMarkAsModified()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository.UnitOfWork.Update(customer);
            customerRepository.UnitOfWork.Commit();
            var customerRepo = customerRepository.Find(1L);
            customerRepo.Name = "updated";
            var updates = projectsContext.GetModified<Customer>();

            updates.Should().NotBeEmpty()
                .And.HaveCount(1);
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestMarkAsDeleted()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository.UnitOfWork.Update(customer);
            customerRepository.UnitOfWork.Commit();
            var customerRepo = customerRepository.Find(1L);
            customerRepository.Delete(customerRepo);
            var updates = projectsContext.GetDeleted<Customer>();

            updates.Should().NotBeEmpty()
                .And.HaveCount(1);
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestAttach()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository.UnitOfWork.Attach(customer1);

            customerRepository.UnitOfWork.Local<Customer>().Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainItemsAssignableTo<Customer>();
        }

        [Fact]
        [Trait("Command", "UnitOfWork")]
        public void TestRollback()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository.Add(customer1);
            customerRepository.UnitOfWork.Commit();
            customer1.Name = "CustomerChanged";
            customerRepository.UnitOfWork.Rollback();

            customerRepository.UnitOfWork.Local<Customer>().Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainItemsAssignableTo<Customer>();
            customer1.Name.Should().Be("Customer1");
        }
    }
}
