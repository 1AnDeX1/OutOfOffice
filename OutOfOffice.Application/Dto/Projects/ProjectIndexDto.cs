using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;
using OutOfOffice.Application.Dto.Employees;

namespace OutOfOffice.Application.Dto.Projects
{
    public class ProjectIndexDto
    {
        public int ID { get; set; }

        public ProjectType ProjectType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int ProjectManagerId { get; set; }

        public string? Comment { get; set; }

        public ActivityStatus Status { get; set; }

        public List<ProjectEmployeeDto>? AssignedEmployees { get; set; }
    }
}
