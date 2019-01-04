using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.EFCore.Behaviours.Auditable
{
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
        
        public string CreatedByName { get; set; }
        public string CreatedAtName { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedAtName { get; set; }

        public Func<DateTime> UpdatedAtAction { get; set; }
        public Func<DateTime> CreatedAtAction { get; set; }
        public Func<string> UpdatedByAction { get; set; }
        public Func<string> CreatedByAction { get; set; }
    }
}
