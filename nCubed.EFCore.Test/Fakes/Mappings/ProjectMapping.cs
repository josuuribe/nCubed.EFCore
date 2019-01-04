using nCubed.EFCore.Test.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    public class ProjectMapping : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("PROJECTS");
            builder.HasKey(c => c.ProjectId);

            builder.Property(c => c.ProjectId).HasColumnName("PROJECT_ID");
            builder.Property(c => c.Name).IsRequired().HasColumnName("NAME");
            builder.Property(c => c.Description).HasColumnName("DESCRIPTION");
            builder.Property(c => c.End).HasColumnName("END_DATE");
            builder.Property(c => c.Start).IsRequired().HasColumnName("START_DATE");

            builder.Property<long>("CUSTOMER_ID");

            builder.HasMany(c => c.ProjectResources).WithOne(c => c.Project).HasForeignKey("PROJECT_ID");
            builder.HasOne(c => c.ProjectDetail).WithOne(C => C.Project);
            builder.HasOne(x => x.Customer)
                .WithMany(y => y.Projects)
                .HasForeignKey("CUSTOMER_ID")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
