using nCubed.EFCore.Test.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    public class ProjectResourceMapping : IEntityTypeConfiguration<ProjectResource>
    {
        public void Configure(EntityTypeBuilder<ProjectResource> builder)
        {
            builder.ToTable("PROJECT_RESOURCES");

            builder.Property<long>("PROJECT_ID");
            builder.Property<long>("RESOURCE_ID");

            builder.HasKey("PROJECT_ID", "RESOURCE_ID");

            builder.Property(c => c.Role).HasColumnName("ROLE_ID");

            builder.HasOne(c => c.Resource).WithMany(d => d.ProjectResources).HasForeignKey("RESOURCE_ID");
            builder.HasOne(c => c.Project).WithMany(d => d.ProjectResources).HasForeignKey("PROJECT_ID");
        }
    }
}
