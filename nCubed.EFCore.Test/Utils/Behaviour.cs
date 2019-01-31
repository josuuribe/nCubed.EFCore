using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using nCubed.EFCore.Extensions;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Fakes;
using nCubed.EFCore.Test.Fakes.Behaviours;
using nCubed.EFCore.Test.Fakes.Repositories;
using NSubstitute;
using System;
using Xunit;

namespace nCubed.EFCore.Test.Utils
{
    public class Behaviour
    {
        private readonly CustomerAuditingRepository customerRepository1 = null;
        private readonly CustomerAuditingRepository2 customerRepository2 = null;

        public Behaviour()
        {
            var connection1 = new SqliteConnection("DataSource=:memory:");
            connection1.Open();
            var dbContextOptions1 = new DbContextOptionsBuilder<ProjectsAuditingContext>()
                .UseSqlite(connection1)
                .Options;
            using (var context1 = new ProjectsAuditingContext(dbContextOptions1))
            {
                context1.Database.EnsureCreated();
            }
            customerRepository1 = new CustomerAuditingRepository(new ProjectsAuditingContext(dbContextOptions1));

            var connection2 = new SqliteConnection("DataSource=:memory:");
            connection2.Open();
            var dbContextOptions2 = new DbContextOptionsBuilder<ProjectsAuditingContext2>()
                .UseSqlite(connection2)
                .Options;
            using (var context2 = new ProjectsAuditingContext2(dbContextOptions2))
            {
                context2.Database.EnsureCreated();
            }
            customerRepository2 = new CustomerAuditingRepository2(new ProjectsAuditingContext2(dbContextOptions2));
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestAuditIsNull()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var dbContextOptions = new DbContextOptionsBuilder<ProjectsNullAuditingContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new ProjectsNullAuditingContext(dbContextOptions))
            {
                context.Audit = new Audit();
                context.Database.EnsureCreated();
                var customerRepository = new CustomerAuditingRepository3(context);
                ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
                var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };
                customerRepository.Add(customer);
                context.Audit = null;

                var ex = Assert.Throws<ArgumentNullException>(() => customerRepository.UnitOfWork.Commit());
                ex.ParamName.Should().Be("audit");
            }
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestAuditWrongColumnCreated()
        {
            string wrongCreatedAtColumnName = "CreatedAtWrong";
            string wrongCreatedByColumnName = "CreatedByWrong";

            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository2.Add(customer);
            customerRepository2.UnitOfWork.Commit();

            Exception ex1 = Assert.Throws<ArgumentException>(() => customerRepository2.GetAuditDate(customer, wrongCreatedAtColumnName));
            ex1.Message.Should().Be($"Column name '{wrongCreatedAtColumnName}' does not exist.");
            Exception ex2 = Assert.Throws<ArgumentException>(() => customerRepository2.GetAuditUser(customer, wrongCreatedByColumnName));
            ex2.Message.Should().Be($"Column name '{wrongCreatedByColumnName}' does not exist.");
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestAuditCreatedCurrentValueIsNull()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            var date = customerRepository2.GetAuditDate(customer, "CreatedAt");
            var user = customerRepository2.GetAuditUser(customer, "CreatedBy");

            date.Should().Be(DateTime.MinValue);
            user.Should().BeNull();
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestAuditCreated()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository2.Add(customer);
            customerRepository2.UnitOfWork.Commit();

            var date = customerRepository2.GetAuditDate(customer, "CreatedAt");
            var user = customerRepository2.GetAuditUser(customer, "CreatedBy");

            date.Should().BeCloseTo(DateTime.Now, 5000);
            user.Should().NotBeNull().And.Equals("Me");
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestAuditWrongColumnUpdated()
        {
            string wrongUpdatedAtColumnName = "UpdatedAtWrong";
            string wrongUpdatedByColumnName = "UpdatedByWrong";

            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository2.Add(customer);
            customerRepository2.UnitOfWork.Commit();

            Exception ex1 = Assert.Throws<ArgumentException>(() => customerRepository2.GetAuditDate(customer, wrongUpdatedAtColumnName));
            ex1.Message.Should().Be($"Column name '{wrongUpdatedAtColumnName}' does not exist.");
            Exception ex2 = Assert.Throws<ArgumentException>(() => customerRepository2.GetAuditUser(customer, wrongUpdatedByColumnName));
            ex2.Message.Should().Be($"Column name '{wrongUpdatedByColumnName}' does not exist.");
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestAuditUpdated()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository2.Add(customer);
            customerRepository2.UnitOfWork.Commit();
            customer.Name = "Customer Modified";
            customerRepository2.UnitOfWork.Commit();

            var date = customerRepository2.GetAuditDate(customer, "UpdatedAt");
            var user = customerRepository2.GetAuditUser(customer, "UpdatedBy");

            date.Should().BeCloseTo(DateTime.Now, 5000);
            user.Should().NotBeNull().And.Equals("Yours");
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestDefaultAuditCreated()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository1.Add(customer);
            customerRepository1.UnitOfWork.Commit();

            var date = customerRepository1.GetAuditDate(customer, "CREATED_AT");
            var user = customerRepository1.GetAuditUser(customer, "CREATED_BY");

            date.Should().BeCloseTo(DateTime.UtcNow, 5000);
            user.Should().NotBeNull().And.Equals(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        }

        [Fact]
        [Trait("Behaviour", "Auditing")]
        public void TestDefaultAuditUpdated()
        {
            ContactInformation contactInformation1 = new ContactInformation() { Phone = "phone", Email = "demo@demo.es" };
            var customer = new Customer() { Name = "Customer1", ContactInformation = contactInformation1 };

            customerRepository1.Add(customer);
            customerRepository1.UnitOfWork.Commit();
            customer.Name = "Customer Modified";
            customerRepository1.UnitOfWork.Commit();

            var date = customerRepository1.GetAuditDate(customer, "UPDATED_AT");
            var user = customerRepository1.GetAuditUser(customer, "UPDATED_BY");

            date.Should().BeCloseTo(DateTime.UtcNow, 5000);
            user.Should().NotBeNull().And.Equals(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        }

        public void Dispose()
        {
            customerRepository1.Dispose();
            customerRepository2.Dispose();
        }
    }
}
