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

namespace OutOfOffice.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly OutOfOfficeDbContext _context;

        public LeaveRequestService(OutOfOfficeDbContext context) 
        {
            _context = context;
        }
        public async Task<IList<LeaveRequest>> GetAllAsync()
        {
            return await _context.LeaveRequests.ToListAsync();
        }

        private IQueryable<LeaveRequest> GetAllSortedWithoutToList(string sort)
        {
            IQueryable<LeaveRequest> leaveRequests = _context.LeaveRequests;
            leaveRequests = sort switch
            {
                "ID" => leaveRequests.OrderBy(lr => lr.ID),
                "ID_desc" => leaveRequests.OrderByDescending(lr => lr.ID),
                "absenceReason_desc" => leaveRequests.OrderByDescending(lr => lr.AbsenceReason),
                "StartDate" => leaveRequests.OrderBy(lr => lr.StartDate),
                "startDate_desc" => leaveRequests.OrderByDescending(lr => lr.StartDate),
                "EndDate" => leaveRequests.OrderBy(lr => lr.EndDate),
                "endDate_desc" => leaveRequests.OrderByDescending(lr => lr.EndDate),
                "Status" => leaveRequests.OrderBy(lr => lr.Status),
                "status_desc" => leaveRequests.OrderByDescending(lr => lr.Status),
                _ => leaveRequests.OrderBy(lr => lr.AbsenceReason),
            };

            return leaveRequests;
        }

        public async Task<IList<LeaveRequest>> GetAllSortedAsync(string sort)
        {
            var sortedLeaveRequests = GetAllSortedWithoutToList(sort);
            return await sortedLeaveRequests.ToListAsync();
        }

        public async Task<IList<LeaveRequest>> GetAllSortedAndFilteredAsync(string sort, LeaveRequestSortItems leaveRequestSortItems)
        {
            IQueryable<LeaveRequest> leaveRequests = GetAllSortedWithoutToList(sort);

            if (leaveRequestSortItems.EmployeeId.HasValue)
            {
                leaveRequests = leaveRequests.Where(lr => lr.EmployeeId == leaveRequestSortItems.EmployeeId.Value);
            }
            if (!string.IsNullOrEmpty(leaveRequestSortItems.AbsenceReason))
            {
                leaveRequests = leaveRequests.Where(lr => lr.AbsenceReason.ToLower().StartsWith(leaveRequestSortItems.AbsenceReason.ToLower()));
            }
            if (leaveRequestSortItems.StartDate.HasValue)
            {
                leaveRequests = leaveRequests.Where(lr => lr.StartDate >= leaveRequestSortItems.StartDate.Value);
            }
            if (leaveRequestSortItems.EndDate.HasValue)
            {
                leaveRequests = leaveRequests.Where(lr => lr.EndDate <= leaveRequestSortItems.EndDate.Value);
            }
            if (leaveRequestSortItems.Status != null)
            {
                leaveRequests = leaveRequests.Where(e => e.Status == leaveRequestSortItems.Status);
            }

            return await leaveRequests.ToListAsync();
        }


        public async Task CreateAsync(LeaveRequest leaveRequest)
        {
            if (!await EmployeeExistsAsync(leaveRequest.EmployeeId))
            {
                throw new Exception("Employee does not exist.");
            }

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests.FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task UpdateAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeExistsAsync(int employeeId)
        {
            return await _context.Employees.AnyAsync(e => e.ID == employeeId);
        }
    }
}
