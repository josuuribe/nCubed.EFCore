using nCubed.EFCore.Behaviours.Auditable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace nCubed.EFCore.Test.Entities
{
    public class Project : IAuditable
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? End { get; set; }
        public DateTime Start { get; set; }
        //One-To-One
        public ProjectDetail ProjectDetail { get; set; }
        // Many To One
        public Customer Customer { get; set; }
        // One To Many
        public ICollection<ProjectResource> ProjectResources { get; private set; } = new HashSet<ProjectResource>();
        public IEnumerable<ProjectResource> Testers => ProjectResources.Where(x => x.Role == Role.Tester);
        public IEnumerable<ProjectResource> Developers => ProjectResources.Where(x => x.Role == Role.Developer);
        public ProjectResource ProjectManager => ProjectResources.SingleOrDefault(x => x.Role == Role.ProjectManager);

        public void AddResource(Resource resource, Role role)
        {
            resource.ProjectResources.Add(new ProjectResource()
            { Project = this, Resource = resource, Role = role });
        }

        public override string ToString() => Name;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((this.End != null) && (this.End < this.Start))
            {
                yield return new ValidationResult("End date is prior to Start date", new[] { "End" });
            }
        }
    }
}
