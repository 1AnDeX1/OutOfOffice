using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Dto.LeaveRequests
{
    public class LeaveRequestIndexDto
    {
        public int ID { get; set; }

        public int EmployeeId { get; set; }

        public AbsenceReason AbsenceReason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Comment { get; set; }

        public RequestStatus Status { get; set; }
    }
}
