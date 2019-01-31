using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nCubed.EFCore.Behaviours.Auditable
{
    public interface IAudit
    {
        /// <summary>
        /// Defines the column name that will store the datetime when row has been created and the action to perform to fulfill the column.
        /// </summary>
        /// <param name="columnName">Created at column name, by default CREATED_AT.</param>
        /// <param name="function">Action that will be executed to fill the column, by default DateTime.UtcNow.</param>
        /// <returns>This audit.</returns>
        IAudit WithCreatedAt(string columnName = "", Func<DateTime> function = null);
        /// <summary>
        /// Defines the column name that will store the user that has created this row and the action to perform to fulfill the column.
        /// </summary>
        /// <param name="columnName">Created at column name, by default CREATED_BY.</param>
        /// <param name="function">Action that will be executed to fill the column, by default WindowsIdentity.GetCurrent().Name.</param>
        /// <returns>This audit.</returns>
        IAudit WithCreatedBy(string columnName = "", Func<string> createdBy = null);
        /// <summary>
        /// Defines the column name that will store the datetime row has been updated and the action to perform to fulfill the column.
        /// </summary>
        /// <param name="columnName">Created at column name, by default UPDATED_AT.</param>
        /// <param name="function">Action that will be executed to fill the column, by default DateTime.UtcNow.</param>
        /// <returns>This audit.</returns>
        IAudit WithUpdatedAt(string columName = "", Func<DateTime> function = null);
        /// <summary>
        /// Defines the column name that will store the user that has updated this row and the action to perform to fulfill the column.
        /// </summary>
        /// <param name="columnName">Updated at column name, by default UPDATED_BY.</param>
        /// <param name="function">Action that will be executed to fill the column, by default WindowsIdentity.GetCurrent().Name.</param>
        /// <returns>This audit.</returns>
        IAudit WithUpdatedBy(string columName = "", Func<string> function = null);
        /// <summary>
        /// Creates all relevant columns in database to store audit information, if no other methods are called all columns will be created with default values.
        /// </summary>
        IAudit Build();
        /// <summary>
        /// Fills database with audit information.
        /// </summary>
        /// <param name="changeTracker">Change tracker to get modified entities.</param>
        void Fill(ChangeTracker changeTracker);
    }
}