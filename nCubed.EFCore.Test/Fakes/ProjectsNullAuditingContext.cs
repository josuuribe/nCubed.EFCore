using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using nCubed.EFCore.Behaviours.Auditable;
using nCubed.EFCore.Extensions;
using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Infrastructure.Mappings;
using System.IO;

namespace nCubed.EFCore.Test.Fakes
{
    public class ProjectsNullAuditingContext : UnitOfWork
    {
        internal IAudit Audit { get; set; }

        public DbSet<Resource> Resources { get; private set; }
        public DbSet<Project> Projects { get; private set; }
        public DbSet<Customer> Customers { get; private set; }
        public DbSet<Technology> Technologies { get; private set; }

        public ProjectsNullAuditingContext(DbContextOptions<ProjectsNullAuditingContext> options) : base(options)
        {
        }


        public ProjectsNullAuditingContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging(true);
#endif
            if (!optionsBuilder.IsConfigured)
            {
                var configurationBuilder = new ConfigurationBuilder()
                                                .SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appSettings.json", false);

                var configuration = configurationBuilder.Build();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerMapping());
            modelBuilder.ApplyConfiguration(new ProjectDetailMapping());
            modelBuilder.ApplyConfiguration(new ProjectMapping());
            modelBuilder.ApplyConfiguration(new ProjectResourceMapping());
            modelBuilder.ApplyConfiguration(new ResourceMapping());
            modelBuilder.ApplyConfiguration(new TechnologyMapping());
            modelBuilder.ApplyConfiguration(new TechnologyResourceMapping());

            if (Audit == null)
            {
                Audit = (null as ModelBuilder).UseAudit().Build();
            }
        }

        public override int SaveChanges()
        {
            (null as ChangeTracker).Fill(Audit);
            return base.SaveChanges();
        }
    }
}
