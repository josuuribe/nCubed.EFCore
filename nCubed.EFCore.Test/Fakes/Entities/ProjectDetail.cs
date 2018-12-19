using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nCubed.EFCore.Test.Entities
{
    public class ProjectDetail
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public decimal Budget { get; set; }
        public bool Critical { get; set; }
    }
}
