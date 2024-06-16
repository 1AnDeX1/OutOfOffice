using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Dto.Projects
{
    public class ProjectCreateDto
    {
        [Required]
        public ProjectType ProjectType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public int ProjectManagerId { get; set; }
        public string Comment { get; set; }
        [Required]
        public ActivityStatus Status { get; set; }
    }

}
