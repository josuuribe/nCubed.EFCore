using nCubed.EFCore.Test.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    public class ResourceMapping : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("RESOURCES");
            builder.HasKey(c => c.ResourceId);
            builder.Property(c => c.ResourceId).HasColumnName("RESOURCE_ID");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(50).HasColumnName("NAME");
            builder.OwnsOne(c => c.ContactInformation).Property(c => c.Phone).HasColumnName("PHONE");
            builder.OwnsOne(c => c.ContactInformation).Property(c => c.Email).HasColumnName("EMAIL");

            builder.HasMany(c => c.ProjectResources).WithOne(d => d.Resource).HasForeignKey("RESOURCE_ID");
            builder.HasMany(c => c.TechnologyResources).WithOne(d => d.Resource).HasForeignKey("RESOURCE_ID");
        }
    }
}
