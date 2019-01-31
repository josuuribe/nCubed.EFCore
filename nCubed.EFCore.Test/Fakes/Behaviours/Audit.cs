using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using nCubed.EFCore.Behaviours.Auditable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace nCubed.EFCore.Test.Fakes.Behaviours
{
    internal class Audit : IAudit
    {
        private Func<string> createdBy;
        private string createdByColumnName;
        private Func<DateTime> createdAt;
        private string createdAtColumnName;
        private Func<string> updatedBy;
        private string updatedByColumnName;
        private Func<DateTime> updatedAt;
        private string updatedAtColumnName;
        private ModelBuilder modelBuilder;
        private bool useDefault = true;

        internal ModelBuilder ModelBuilder
        {
            get
            {
                return this.modelBuilder;
            }
            set
            {
                this.modelBuilder = value;
            }
        }

        public IAudit WithCreatedBy(string columnName = "", Func<string> createdBy = null)
        {
            this.useDefault = false;
            if (String.IsNullOrEmpty(this.createdByColumnName))
            {
                this.createdByColumnName = "CREATED_BY";
            }
            else
            {
                this.createdByColumnName = columnName;
            }
            if (createdBy == null)
            {
                this.createdBy = () => System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            else
            {
                this.createdBy = createdBy;
            }
            return this;
        }

        public IAudit WithCreatedAt(string columnName = "", Func<DateTime> createdAt = null)
        {
            this.useDefault = false;
            if (String.IsNullOrEmpty(this.createdAtColumnName))
            {
                this.createdAtColumnName = "CREATED_AT";
            }
            else
            {
                this.createdAtColumnName = columnName;
            }
            if (createdAt == null)
            {
                this.createdAt = () => DateTime.UtcNow;
            }
            else
            {
                this.createdAt = createdAt;
            }
            return this;
        }

        public IAudit WithUpdatedAt(string columnName = "", Func<DateTime> function = null)
        {
            this.useDefault = false;
            if (String.IsNullOrEmpty(this.updatedAtColumnName))
            {
                this.updatedAtColumnName = "UPDATED_AT";
            }
            else
            {
                this.updatedAtColumnName = columnName;
            }
            if (updatedAt == null)
            {
                this.updatedAt = () => DateTime.UtcNow;
            }
            else
            {
                this.updatedAt = function;
            }
            return this;
        }

        public IAudit WithUpdatedBy(string columnName = "", Func<string> function = null)
        {
            this.useDefault = false;
            if (String.IsNullOrEmpty(this.updatedByColumnName))
            {
                this.updatedByColumnName = "UPDATED_BY";
            }
            else
            {
                this.updatedByColumnName = columnName;
            }
            if (updatedBy == null)
            {
                this.updatedBy = () => System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            else
            {
                this.updatedBy = function;
            }
            return this;
        }

        public IAudit Build()
        {
            if (useDefault)
            {
                this.WithCreatedAt().WithCreatedBy().WithUpdatedAt().WithUpdatedBy();
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(x => typeof(IAuditable).IsAssignableFrom(x.ClrType)))
            {
                if (!String.IsNullOrEmpty(createdByColumnName))
                    entity.AddProperty(createdByColumnName, typeof(string)).SetMaxLength(50);
                if (!String.IsNullOrEmpty(createdAtColumnName))
                    entity.AddProperty(createdAtColumnName, typeof(DateTime));
                if (!String.IsNullOrEmpty(updatedByColumnName))
                    entity.AddProperty(updatedByColumnName, typeof(string)).SetMaxLength(50);
                if (!String.IsNullOrEmpty(updatedAtColumnName))
                    entity.AddProperty(updatedAtColumnName, typeof(DateTime?));
            }
            return this;
        }

        public void Fill(ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (!String.IsNullOrEmpty(createdByColumnName))
                                entry.Property(createdByColumnName).CurrentValue = createdBy();
                            if (!String.IsNullOrEmpty(createdAtColumnName))
                                entry.Property(createdAtColumnName).CurrentValue = createdAt();
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            if (!String.IsNullOrEmpty(updatedByColumnName))
                                entry.Property(updatedByColumnName).CurrentValue = updatedBy();
                            if (!String.IsNullOrEmpty(updatedAtColumnName))
                                entry.Property(updatedAtColumnName).CurrentValue = updatedAt();
                            break;
                    }
                }
            }
        }
    }
}
