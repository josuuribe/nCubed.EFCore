using nCubed.EFCore.Test.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    public class ProjectDetailMapping : IEntityTypeConfiguration<ProjectDetail>
    {
        public void Configure(EntityTypeBuilder<ProjectDetail> builder)
        {
            builder.ToTable("PROJECT_DETAILS");
            builder.HasKey(c => c.ProjectId);
            builder.Property(c => c.ProjectId).HasColumnName("PROJECT_ID");

            builder.Property(c => c.Budget).HasColumnName("BUDGET");
            builder.Property(c => c.Critical).HasColumnName("CRITICAL");

            builder.HasOne(c => c.Project).WithOne(d => d.ProjectDetail);
        }
    }
}
