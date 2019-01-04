using nCubed.EFCore.Test.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("CUSTOMERS");
            builder.HasKey(c => c.CustomerId);
            builder.Property(c => c.CustomerId).HasColumnName("CUSTOMER_ID");

            builder.Property(c => c.CustomerId).HasColumnName("CUSTOMER_ID");
            builder.Property(c => c.Name).IsRequired().HasColumnName("NAME");

            builder.OwnsOne(c => c.ContactInformation).Property(c => c.Phone).HasColumnName("PHONE");
            builder.OwnsOne(c => c.ContactInformation).Property(c => c.Email).HasColumnName("EMAIL");
            builder.HasMany(c => c.Projects).WithOne(d => d.Customer);

            builder.HasOne(c => c.ContactInformation).WithOne().HasForeignKey<ContactInformation>();
        }
    }
}
