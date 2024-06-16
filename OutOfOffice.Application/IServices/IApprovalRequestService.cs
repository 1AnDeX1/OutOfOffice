using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.IServices
{
    public interface IApprovalRequestService
    {
        Task<IList<ApprovalRequest>> GetAllSortedAsync(string sort);
        Task<IList<ApprovalRequest>> GetAllSortedAndFilteredAsync(string sort, ApprovalRequestSortItems approvalRequestSortItems);
    }
}
