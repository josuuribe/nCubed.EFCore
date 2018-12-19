using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Test.Infrastructure.Mappings
{
    class TechnologyMapping : IEntityTypeConfiguration<Technology>
    {
        public void Configure(EntityTypeBuilder<Technology> builder)
        {
            builder.ToTable("TECHNOLOGY");
            builder.HasKey(c => c.TechnologyId);
            builder.Property(c => c.TechnologyId).HasColumnName("TECHNOLOGY_ID");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(50).HasColumnName("NAME");

            builder.HasMany(c => c.TechnologyResources).WithOne(d => d.Technology);
        }
    }
}
