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

namespace nCubed.EFCore.Test.EntityFramework
{
    public class UnitOfWork : Context
    {
        public UnitOfWork() { }

        [Fact]
        public void TestTrackedEntitiesByType()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
            ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer2 = new Customer() { Name = "Customer1", ContactInformation = contactInformation2 };
            Technology technology = new Technology();
            technology.Name = "Name";

            var context = new ProjectsContext(dbContextOptions);

            CustomerRepository customerRepository = new CustomerRepository(context);
            customerRepository.Add(customer1);
            customerRepository.Add(customer2);
            TechnologyRepository technologyRepository = new TechnologyRepository(context);
            technologyRepository.Add(technology);

            var customersTracked = customerRepository.UnitOfWork.Local<Customer>();

            customersTracked.Should().NotBeEmpty()
                    .And.HaveCount(2)
                    .And.ContainItemsAssignableTo<Customer>();
        }

        [Fact]
        public void TestTrackedEntities()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
            ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone2", Email = "demo2@demo.es" };
            var customer2 = new Customer() { Name = "Customer2", ContactInformation = contactInformation2 };
            Technology technology = new Technology();
            technology.Name = "Name";

            var context = new ProjectsContext(dbContextOptions);

            CustomerRepository customerRepository = new CustomerRepository(context);
            customerRepository.Add(customer1);
            customerRepository.Add(customer2);
            TechnologyRepository technologyRepository = new TechnologyRepository(context);
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
        public void TestReset()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone1", Email = "demo1@demo.es" };
            var customer1 = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };


            ProjectsContext projectsContext = new ProjectsContext(dbContextOptions);
            CustomerRepository customerRepository = new CustomerRepository(projectsContext);
            customerRepository.Add(customer1);
            customerRepository.UnitOfWork.Commit();

            customer1.Name = "Changed name1";
            customerRepository.UnitOfWork.Reset();


            customer1.Name.Should().Be("Customer1", "Name different.");
            customerRepository.UnitOfWork.Local<Customer>().Should().BeEmpty();
        }
    }
}
