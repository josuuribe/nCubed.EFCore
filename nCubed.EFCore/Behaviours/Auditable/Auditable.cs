using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Behaviours.Auditable
{
    /// <summary>
    /// Class to configure auditing.
    /// </summary>
    public class Auditable
    {
        public Auditable()
        {
            this.CreatedByName = "CREATED_BY";
            this.CreatedAtName = "CREATED_AT";
            this.UpdatedAtName = "UPDATED_AT";
            this.UpdatedByName = "UPDATED_BY";
            DateTime now() => DateTime.UtcNow;
            string user() => System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            this.CreatedAtAction = now;
            this.CreatedByAction = user;
            this.UpdatedAtAction = now;
            this.UpdatedByAction = user;
        }
        /// <summary>
        /// Created by field name.
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Created at field name.
        /// </summary>
        public string CreatedAtName { get; set; }
        /// <summary>
        /// Updated at field name.
        /// </summary>
        public string UpdatedByName { get; set; }
        /// <summary>
        /// Updated at field name.
        /// </summary>
        public string UpdatedAtName { get; set; }
        /// <summary>
        /// Function that sets a value for UpdatedAt field, by default current Utc time.
        /// </summary>
        public Func<DateTime> UpdatedAtAction { get; set; }
        /// <summary>
        /// Function that sets a value for CreatedAt field, by default current Utc time.
        /// </summary>
        public Func<DateTime> CreatedAtAction { get; set; }
        /// <summary>
        /// Function that sets a value for UpdatedBy field, by default current user.
        /// </summary>
        public Func<string> UpdatedByAction { get; set; }
        /// <summary>
        /// Function that sets a value for CreatedBy field, by default current user.
        /// </summary>
        public Func<string> CreatedByAction { get; set; }
    }
}
