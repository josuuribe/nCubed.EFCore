using FluentAssertions;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using nCubed.EFCore.Extensions;
using System.Linq;
using nCubed.EFCore.Test.Fakes;
using nCubed.EFCore.Test.Repositories.Fakes;

namespace nCubed.EFCore.Test.EntityFramework
{
    public class Pagination : Context
    {
        private readonly List<Customer> customers = null;
        private readonly CustomerRepository customerRepository = null;
        public Pagination()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            ContactInformation contactInformation2 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            ContactInformation contactInformation3 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            ContactInformation contactInformation4 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            ContactInformation contactInformation5 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            customers = new List<Customer>{
                new Customer() { Name = "Customer1", ContactInformation = contactInformation1 },
                new Customer() { Name = "Customer2", ContactInformation = contactInformation2 },
                new Customer() { Name = "Customer3", ContactInformation = contactInformation3 },
                new Customer() { Name = "Customer4", ContactInformation = contactInformation4 },
                new Customer() { Name = "Customer5", ContactInformation = contactInformation5 } };

            customerRepository = new CustomerRepository(new ProjectsContext(dbContextOptions));

            customerRepository.AddRange(customers);
            customerRepository.UnitOfWork.Commit();
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakePageAscendingQueryable()
        {
            var paged = customerRepository.GetPaged(0, 3, c => c.CustomerId, true).ToList();

            paged.Count().Should().Be(3);
            paged.ElementAt(0).Name.Should().Be("Customer1");
            paged.ElementAt(1).Name.Should().Be("Customer2");
            paged.ElementAt(2).Name.Should().Be("Customer3");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakePageDescendingQueryable()
        {
            var paged = customerRepository.GetPaged(0, 3, c => c.CustomerId, false).ToList();

            paged.Count().Should().Be(3);
            paged.ElementAt(0).Name.Should().Be("Customer5");
            paged.ElementAt(1).Name.Should().Be("Customer4");
            paged.ElementAt(2).Name.Should().Be("Customer3");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakePageAscending()
        {
            var paged = customerRepository.GetPaged(0, 3, "CustomerId ascending").ToList();

            paged.Count().Should().Be(3);
            paged.ElementAt(0).Name.Should().Be("Customer1");
            paged.ElementAt(1).Name.Should().Be("Customer2");
            paged.ElementAt(2).Name.Should().Be("Customer3");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakePageDescending()
        {
            var paged = customerRepository.GetPaged(0, 3, "CustomerId descending").ToList();

            paged.Count().Should().Be(3);
            paged.ElementAt(0).Name.Should().Be("Customer5");
            paged.ElementAt(1).Name.Should().Be("Customer4");
            paged.ElementAt(2).Name.Should().Be("Customer3");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakeFirstPageQueryable()
        {
            var paged = customerRepository.GetPaged(0, 3, (c) => c.CustomerId).ToList();

            paged.Count().Should().Be(3);
            paged.ElementAt(0).Name.Should().Be("Customer1");
            paged.ElementAt(1).Name.Should().Be("Customer2");
            paged.ElementAt(2).Name.Should().Be("Customer3");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakeFirstPage()
        {
            var paged = customerRepository.GetPaged(0, 3, nameof(Customer.CustomerId)).ToList();

            paged.Count().Should().Be(3);
            paged.ElementAt(0).Name.Should().Be("Customer1");
            paged.ElementAt(1).Name.Should().Be("Customer2");
            paged.ElementAt(2).Name.Should().Be("Customer3");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakeNextPageQueryable()
        {
            var paged = customerRepository.GetPaged(1, 3, (c) => c.CustomerId).ToList();

            paged.Count().Should().Be(2);
            paged.ElementAt(0).Name.Should().Be("Customer4");
            paged.ElementAt(1).Name.Should().Be("Customer5");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakeNextPage()
        {
            var paged = customerRepository.GetPaged(1, 3, nameof(Customer.CustomerId)).ToList();

            paged.Count().Should().Be(2);
            paged.ElementAt(0).Name.Should().Be("Customer4");
            paged.ElementAt(1).Name.Should().Be("Customer5");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakeLastPageQueryable()
        {
            var paged = customerRepository.GetPaged(2, 2, (c) => c.CustomerId).ToList();

            paged.Count().Should().Be(1);
            paged.ElementAt(0).Name.Should().Be("Customer5");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void TakeLastPage()
        {
            var paged = customerRepository.GetPaged(2, 2, nameof(Customer.CustomerId)).ToList();

            paged.Count().Should().Be(1);
            paged.ElementAt(0).Name.Should().Be("Customer5");
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void AllItemsInFirstPageQueryable()
        {
            var paged = customerRepository.GetPaged(0, int.MaxValue, (c) => c.CustomerId).ToList();

            paged.Count().Should().Be(5);
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void AllItemsInFirstPage()
        {
            var paged = customerRepository.GetPaged(0, int.MaxValue, nameof(Customer.CustomerId)).ToList();

            paged.Count().Should().Be(5);
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void NumberOfItemsNotSpecifiedReturnsZeroQueryable()
        {
            var paged = customerRepository.GetPaged(0, 0, (c) => c.CustomerId).ToList();

            paged.Count().Should().Be(0);
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void NumberOfItemsNotSpecifiedReturnsZero()
        {
            var paged = customerRepository.GetPaged(0, 0, nameof(Customer.CustomerId)).ToList();

            paged.Count().Should().Be(0);
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void MaxPageReturnsZeroQueryable()
        {
            var paged = customerRepository.GetPaged(int.MaxValue, 1, (c) => c.CustomerId).ToList();

            paged.Count().Should().Be(0);
        }

        [Fact]
        [Trait("Pagination", "Repository")]
        public void MaxPageReturnsZero()
        {
            var paged = customerRepository.GetPaged(int.MaxValue, 1, nameof(Customer.CustomerId)).ToList();

            paged.Count().Should().Be(0);
        }

        void Dispose()
        {
            customerRepository.Dispose();
        }
    }
}
