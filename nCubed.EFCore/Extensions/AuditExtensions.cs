using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using nCubed.EFCore.Behaviours.Auditable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nCubed.EFCore.Extensions
{
    public static class AuditExtensions
    {
        /// <summary>
        /// Fills database with audit information.
        /// </summary>
        /// <param name="changeTracker">Change tracker to get modified entities.</param>
        /// <param name="auditable">Audit configuration information to use.</param>
        public static void FillAudit(this ChangeTracker changeTracker, Auditable auditable = null)
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
        /// <summary>
        /// Creates all relevant columns in database to store audit information.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder in use to create required columns.</param>
        /// <param name="auditable">Audit configuration information to use.</param>
        public static void CreateAudit(this ModelBuilder modelBuilder, Auditable auditable = null)
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
