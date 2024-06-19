using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.IServices
{
    public interface ILeaveRequestService
    {
        Task<IList<LeaveRequest>> GetAllAsync();

        Task<IList<LeaveRequest>> GetAllSortedAsync(string sort);

        Task<IList<LeaveRequest>> GetAllSortedAndFilteredAsync(string sort, LeaveRequestSortItems leaveRequestSortItems);

        Task CreateAsync(LeaveRequest leaveRequest);

        Task<LeaveRequest> GetByIdAsync(int id);

        Task UpdateAsync(LeaveRequest leaveRequest);

        Task SubmitAsync(int leaveRequestId);

        Task CancelAsync(int leaveRequestId);

        IList<LeaveRequest> GetAllForCurrentEmployee(IList<LeaveRequest> leaveRequests, int curUserId);
    }
}
