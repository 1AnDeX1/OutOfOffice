using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Dto.Employees
{
    public class EmployeeUpdateDto
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public Subdivision Subdivision { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        public ActivityStatus Status { get; set; }

        [Required]
        public int PeoplePartnerId { get; set; }

        [Required]
        public int OutOfOfficeBalance { get; set; }

        public string? Photo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
