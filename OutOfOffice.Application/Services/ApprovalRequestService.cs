using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using OutOfOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly OutOfOfficeDbContext _context;

        public ApprovalRequestService(OutOfOfficeDbContext context)
        {
            _context = context;
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
                    approvalRequests = approvalRequests.OrderBy(ar => ar.ID); // Default sorting by ID ascending
                    break;
            }

            return approvalRequests;
        }
    }
}
