using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.SortClasses
{
    public class LeaveRequestSortItems
    {
        public int? RequestNumber { get; set; }
        public int? EmployeeId { get; set; }
        public string AbsenceReason { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Status? Status { get; set; }
    }
}
