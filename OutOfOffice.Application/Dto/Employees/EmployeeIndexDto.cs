using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.Dto.Employees
{
    public class EmployeeIndexDto
    {
        public int ID { get; set; }

        public string FullName { get; set; }

        public string Subdivision { get; set; }

        public string Position { get; set; }

        public string Status { get; set; }

        public int PeoplePartnerId { get; set; }

        public int OutOfOfficeBalance { get; set; }

        public string? Photo { get; set; }
    }
}
