using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using OutOfOffice.Data;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly OutOfOfficeDbContext _context;
        private readonly IEmployeeService _employeeService;

        public ApprovalRequestService(OutOfOfficeDbContext context,
            IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public async Task<IList<ApprovalRequest>> GetAllAsync()
        {
            return await _context.ApprovalRequests.ToListAsync();
        }

        public async Task<IList<ApprovalRequest>> GetAllSortedAsync(string sort)
        {
            var sortedApprovalRequests = GetAllSortedWithoutToList(sort);
            return await sortedApprovalRequests.ToListAsync();
        }

        public async Task<IList<ApprovalRequest>> GetAllSortedAndFilteredAsync(string sort, ApprovalRequestSortItems approvalRequestSortItems)
        {
            IQueryable<ApprovalRequest> approvalRequests = GetAllSortedWithoutToList(sort);

            if (approvalRequestSortItems.ApproverId.HasValue)
            {
                approvalRequests = approvalRequests.Where(ar => ar.ApproverId == approvalRequestSortItems.ApproverId.Value);
            }
            if (approvalRequestSortItems.LeaveRequestId.HasValue)
            {
                approvalRequests = approvalRequests.Where(ar => ar.LeaveRequestId == approvalRequestSortItems.LeaveRequestId.Value);
            }
            if (approvalRequestSortItems.Status != null)
            {
                approvalRequests = approvalRequests.Where(ar => ar.Status == approvalRequestSortItems.Status);
            }

            return await approvalRequests.ToListAsync();
        }

        private IQueryable<ApprovalRequest> GetAllSortedWithoutToList(string sort)
        {
            IQueryable<ApprovalRequest> approvalRequests = _context.ApprovalRequests;

            switch (sort)
            {
                case "ID":
                    approvalRequests = approvalRequests.OrderBy(ar => ar.ID);
                    break;
                case "ID_desc":
                    approvalRequests = approvalRequests.OrderByDescending(ar => ar.ID);
                    break;
                case "ApproverId":
                    approvalRequests = approvalRequests.OrderBy(ar => ar.ApproverId);
                    break;
                case "approverId_desc":
                    approvalRequests = approvalRequests.OrderByDescending(ar => ar.ApproverId);
                    break;
                case "LeaveRequestId":
                    approvalRequests = approvalRequests.OrderBy(ar => ar.LeaveRequestId);
                    break;
                case "leaveRequestId_desc":
                    approvalRequests = approvalRequests.OrderByDescending(ar => ar.LeaveRequestId);
                    break;
                case "Status":
                    approvalRequests = approvalRequests.OrderBy(ar => ar.Status);
                    break;
                case "status_desc":
                    approvalRequests = approvalRequests.OrderByDescending(ar => ar.Status);
                    break;
                default:
                    approvalRequests = approvalRequests.OrderBy(ar => ar.ID);
                    break;
            }

            return approvalRequests;
        }

        public async Task CreateAsync(ApprovalRequest approvalRequest)
        {
            if (approvalRequest.ApproverId != null && !await _employeeService.EmployeeExistsAsync(approvalRequest.ApproverId))
            {
                throw new Exception("Employee does not exist.");
            }
            try
            {
                await _context.ApprovalRequests.AddAsync(approvalRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }   
        }

        public async Task<ApprovalRequest?> GetByIdAsync(int id)
        {
            return await _context.ApprovalRequests.FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task UpdateAsync(ApprovalRequest approvalRequest)
        {
            _context.ApprovalRequests.Update(approvalRequest);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveAsync(int approvalRequestId, int curUserId)
        {
            var approvalRequest = await GetByIdAsync(approvalRequestId);

            if (approvalRequest == null)
                throw new ArgumentException("Approval request not found.");

            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(r => r.ID == approvalRequest.LeaveRequestId);

            if (leaveRequest == null)
                throw new ArgumentException("Leave request not found.");

            var employee = await _employeeService.GetByIdAsync(leaveRequest.EmployeeId);

            if (employee == null)
                throw new ArgumentException("Leave request not found.");

            int days = (int)Math.Ceiling((leaveRequest.EndDate - leaveRequest.StartDate).TotalDays + 1);

            if (employee.OutOfOfficeBalance >= days)
            {
                employee.OutOfOfficeBalance -= days;
                _context.Employees.Update(employee);

                approvalRequest.Status = RequestStatus.Approved;
                leaveRequest.Status = RequestStatus.Approved;
                approvalRequest.ApproverId = curUserId;

                _context.LeaveRequests.Update(leaveRequest);
                _context.ApprovalRequests.Update(approvalRequest);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Insufficient balance to approve the leave request.");
            }
        }

        public async Task RejectAsync(int approvalRequestId, string rejectionComment, int curUserId)
        {
            var approvalRequest = await GetByIdAsync(approvalRequestId);

            if (approvalRequest == null)
            {
                throw new ArgumentException("Approval request not found.");
            }

            approvalRequest.Status = RequestStatus.Rejected;
            approvalRequest.Comment = rejectionComment;
            approvalRequest.ApproverId = curUserId;

            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(r => r.ID == approvalRequest.LeaveRequestId);

            if (leaveRequest == null)
                throw new ArgumentException("Leave request not found.");

            leaveRequest.Status = RequestStatus.Rejected;

            _context.LeaveRequests.Update(leaveRequest);

            await _context.SaveChangesAsync();
        }

    }
}
