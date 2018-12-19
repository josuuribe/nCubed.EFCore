using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using nCubed.EFCore.Behaviours.Auditable;
using Microsoft.EntityFrameworkCore;

namespace nCubed.EFCore.Extensions
{
    public static class AuditExtensions
    {
        public static void FillAudit(this Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker, Behaviours.Auditable.Auditable auditable = null)
        {
            auditable = auditable ?? new Auditable();
            foreach (var entry in changeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditable)
                {
                    switch(entry.State)
                    {
                        case EntityState.Added:
                            if (!String.IsNullOrEmpty(auditable.CreatedByName))
                                entry.Property(auditable.CreatedByName).CurrentValue = auditable.CreatedByAction();
                            if (!String.IsNullOrEmpty(auditable.CreatedAtName))
                                entry.Property(auditable.CreatedAtName).CurrentValue = auditable.CreatedAtAction();
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            if (!String.IsNullOrEmpty(auditable.UpdatedByName))
                                entry.Property(auditable.UpdatedByName).CurrentValue = auditable.UpdatedByAction();
                            if (!String.IsNullOrEmpty(auditable.UpdatedAtName))
                                entry.Property(auditable.UpdatedAtName).CurrentValue = auditable.UpdatedAtAction();
                            break;
                    }
                }
            }
        }

        public static void CreateAudit(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder, Behaviours.Auditable.Auditable auditable = null)
        {
            auditable = auditable ?? new Auditable();
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(x => typeof(IAuditable).IsAssignableFrom(x.ClrType)))
            {
                if (!String.IsNullOrEmpty(auditable.CreatedByName))
                    entity.AddProperty(auditable.CreatedByName, typeof(string)).SetMaxLength(50);
                if (!String.IsNullOrEmpty(auditable.CreatedAtName))
                    entity.AddProperty(auditable.CreatedAtName, typeof(DateTime));
                if (!String.IsNullOrEmpty(auditable.UpdatedByName))
                    entity.AddProperty(auditable.UpdatedByName, typeof(string)).SetMaxLength(50);
                if (!String.IsNullOrEmpty(auditable.UpdatedAtName))
                    entity.AddProperty(auditable.UpdatedAtName, typeof(DateTime?));
            }
        }
    }
}
