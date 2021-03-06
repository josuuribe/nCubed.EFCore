﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.IO;
using Microsoft.Extensions.Configuration.Json;
using System.Data.Common;
using System.Collections.Generic;
using nCubed.EFCore.Repositories;
using nCubed.EFCore.Test.Entities;
using nCubed.EFCore.Test.Infrastructure.Mappings;
using nCubed.EFCore.Extensions;

namespace nCubed.EFCore.Test.Fakes
{
    public class ProjectsContext : UnitOfWork
    {
        public DbSet<Resource> Resources { get; private set; }
        public DbSet<Project> Projects { get; private set; }
        public DbSet<Customer> Customers { get; private set; }
        public DbSet<Technology> Technologies { get; private set; }

        public ProjectsContext(DbContextOptions<ProjectsContext> options) : base(options)
        {
        }


        public ProjectsContext() : base()
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
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
