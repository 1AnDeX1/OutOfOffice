using OutOfOffice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.SortClasses
{
    public class ApprovalRequestSortItems
    {
        public int? ApproverId { get; set; }

        public int? LeaveRequestId { get; set; }

        public RequestStatus? Status { get; set; }
    }
}
