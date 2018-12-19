using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    public class TechnologyResourceMapping : IEntityTypeConfiguration<TechnologyResource>
    {
        public void Configure(EntityTypeBuilder<TechnologyResource> builder)
        {
            builder.ToTable("TECHNOLOGY_RESOURCE");

            builder.Property<int>("RESOURCE_ID");
            builder.Property<int>("TECHNOLOGY_ID");

            builder.HasKey("RESOURCE_ID", "TECHNOLOGY_ID");

            builder.HasOne(c => c.Resource).WithMany(c => c.TechnologyResources).HasForeignKey("RESOURCE_ID");
            builder.HasOne(c => c.Technology).WithMany(c => c.TechnologyResources).HasForeignKey("TECHNOLOGY_ID");
        }
    }
}
