using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using nCubed.EFCore.Behaviours.Auditable;
using nCubed.EFCore.Repositories;
using System;
using System.Linq;

namespace nCubed.EFCore.Extensions
{
    public static class AuditExtensions
    {
        /// <summary>
        /// Fills database with audit information.
        /// </summary>
        /// <param name="changeTracker">Change tracker that stores the entities with audit information.</param>
        /// <param name="audit">Audit objet to use.</param>
        public static void Fill(this ChangeTracker changeTracker, IAudit audit)
        {
            if (audit == null)
            {
                throw new ArgumentNullException($"{nameof(audit)}");
            }

            audit.Fill(changeTracker);
        }
        /// <summary>
        /// Creates all relevant columns for auditing, if not Audit object specified a new one will be created, the default column names and values are:
        /// CREATED_AT using DateTime.UtcNow
        /// CREATED_BY using WindowsIdentity.GetCurrent().Name
        /// UPDATED_AT using DateTime.UtcNow
        /// UPDATED_BY using WindowsIdentity.GetCurrent().Name
        /// </summary>
        /// <param name="modelBuilder">Model builder that created the context.</param>
        /// <param name="audit">Audit objet to use.</param>
        /// <returns>The audit.</returns>
        public static IAudit UseAudit(this ModelBuilder modelBuilder, IAudit audit = null)
        {
            return audit ?? new Audit(modelBuilder);
        }
        /// <summary>
        /// Gets the audit date (Created or Updated).
        /// </summary>
        /// <param name="repository">The repository that hosts this entity.</param>
        /// <param name="entity">Entity with Audit information.</param>
        /// <param name="columnName">Date column name with audit information.</param>
        /// <returns>Audit DateTime.</returns>
        public static DateTime GetAuditDate(this IRepository repository, IAuditable entity, string columnName)
        {
            if (repository.UnitOfWork.Entry(entity).Properties.FirstOrDefault(x => x.Metadata.Name == columnName) == null)
            {
                throw new ArgumentException($"Column name '{columnName}' does not exist.");
            }

            bool b = DateTime.TryParse(repository.UnitOfWork.Entry(entity).Property(columnName).CurrentValue?.ToString(), out DateTime dateTime);
            return b ? dateTime : DateTime.MinValue;
        }
        /// <summary>
        /// Gets the user that created or updated this row.
        /// </summary>
        /// <param name="repository">The repository that hosts this entity.</param>
        /// <param name="entity">Entity with Audit information.</param>
        /// <param name="columnName">Date column name with audit information.</param>
        /// <returns>Audit DateTime.</returns>
        public static string GetAuditUser(this IRepository repository, IAuditable entity, string columnName)
        {
            if (repository.UnitOfWork.Entry(entity).Properties.FirstOrDefault(x => x.Metadata.Name == columnName) == null)
            {
                throw new ArgumentException($"Column name '{columnName}' does not exist.");
            }

            return repository.UnitOfWork.Entry(entity).Property(columnName).CurrentValue?.ToString();
        }
    }
}
