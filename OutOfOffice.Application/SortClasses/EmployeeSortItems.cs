using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.SortClasses
{
    public class EmployeeSortItems
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        public Subdivision? Subdivision { get; set; }

        public Position? Position { get; set; }

        public Status? Status { get; set; }

        [Range(0, 1000000)]
        public int? PeoplePartnerId { get; set; }

        [Range(0, 1000000)]
        public int? OutOfOfficeBalance { get; set; }
    }
}
