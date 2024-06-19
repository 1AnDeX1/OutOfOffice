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
    public class LeaveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public AbsenceReason AbsenceReason { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string? Comment { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.New;

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
