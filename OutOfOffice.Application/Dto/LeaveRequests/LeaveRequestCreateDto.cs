using System.ComponentModel.DataAnnotations;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Dto.LeaveRequests
{
    public class LeaveRequestCreateDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public AbsenceReason AbsenceReason { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.New;
    }
}
