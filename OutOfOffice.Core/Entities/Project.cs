using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Core.Entities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public ProjectType ProjectType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int ProjectManagerId { get; set; }

        [ForeignKey("ProjectManagerId")]
        public Employee ProjectManager { get; set; }

        public string? Comment { get; set; }

        [Required]
        public ActivityStatus Status { get; set; }

        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
