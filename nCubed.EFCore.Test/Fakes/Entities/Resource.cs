using nCubed.EFCore.Test.Entities.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nCubed.EFCore.Test.Entities
{
    public class Resource
    {
        public long ResourceId { get; set; }
        public string Name { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public ICollection<ProjectResource> ProjectResources { get; private set; } = new HashSet<ProjectResource>();
        public ICollection<TechnologyResource> TechnologyResources { get; private set; } = new HashSet<TechnologyResource>();
        public override string ToString() => Name;
    }
}
