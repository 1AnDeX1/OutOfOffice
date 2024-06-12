using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Core.Entities
{
    public class ApprovalRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required]
        public int LeaveRequestId { get; set; }

        [Required]
        public string Status { get; set; } = "New";

        public string Comment { get; set; }

        [ForeignKey("ApproverId")]
        public Employee Approver { get; set; }

        [ForeignKey("LeaveRequestId")]
        public LeaveRequest LeaveRequest { get; set; }
    }
}
