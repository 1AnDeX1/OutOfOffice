using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string AbsenceReason { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; }

        [Required]
        public string Status { get; set; } = "New";

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
