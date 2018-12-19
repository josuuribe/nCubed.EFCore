using nCubed.EFCore.Test.Entities.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nCubed.EFCore.Test.Entities
{
    public class Technology
    {
        public int TechnologyId { get; set; }
        public string Name { get; set; }
        public ICollection<TechnologyResource> TechnologyResources { get; private set; } = new HashSet<TechnologyResource>();

        public override string ToString() => Name;
    }
}
