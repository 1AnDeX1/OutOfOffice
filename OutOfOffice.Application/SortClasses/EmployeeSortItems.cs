using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.SortClasses
{
    public class EmployeeSortItems
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Subdivision { get; set; }

        [MaxLength(100)]
        public string Position { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }

        [Range(0, 1000000)]
        public int? PeoplePartnerId { get; set; }

        [Range(0, 1000000)]
        public int? OutOfOfficeBalance { get; set; }
    }
}
