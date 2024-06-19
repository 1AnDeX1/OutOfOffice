using OutOfOffice.Core.Entities;
using OutOfOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Dto.ApprovalRequest
{
    public class ApprovalRequestIndexDto
    {
        public int ID { get; set; }

        public int? ApproverId { get; set; }

        public int LeaveRequestId { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.New;

        public string Comment { get; set; }
    }
}
