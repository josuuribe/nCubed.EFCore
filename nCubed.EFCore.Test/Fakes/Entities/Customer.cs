using nCubed.EFCore.Behaviours.Auditable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nCubed.EFCore.Test.Entities
{
    public class Customer: IAuditable
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public ContactInformation ContactInformation { get; set; }
        // Many To One, other side
        public ICollection<Project> Projects { get; private set; } = new HashSet<Project>();
        public override string ToString() => Name;
    }
}
