using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class EmployeeService : IEmployeeService
    {
        private readonly OutOfOfficeDbContext _context;

        public EmployeeService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }
        public IQueryable<Employee> GetAllSortedWithouToList(string sort)
        {
            IQueryable<Employee> employees = _context.Employees;
            employees = sort switch
            {
                "name_desc" => employees.OrderByDescending(e => e.FullName),
                "Subdivision" => employees.OrderBy(e => e.Subdivision),
                "subdivision_desc" => employees.OrderByDescending(e => e.Subdivision),
                "Position" => employees.OrderBy(e => e.Position),
                "position_desc" => employees.OrderByDescending(e => e.Position),
                "Status" => employees.OrderBy(e => e.Status),
                "status_desc" => employees.OrderByDescending(e => e.Status),
                "PeoplePartner" => employees.OrderBy(e => e.PeoplePartnerId),
                "peoplepartner_desc" => employees.OrderByDescending(e => e.PeoplePartnerId),
                "Balance" => employees.OrderBy(e => e.OutOfOfficeBalance),
                "balance_desc" => employees.OrderByDescending(e => e.OutOfOfficeBalance),
                _ => employees.OrderBy(e => e.FullName),
            };

            return employees;
        }
        public async Task<IList<Employee>> GetAllSortedAsync(string sort)
        {
            var sortedEmployees = GetAllSortedWithouToList(sort);
            return await sortedEmployees.ToListAsync();
        }
        public async Task<IList<Employee>> GetAllSortedAndFilteredAsync(string sort, EmployeeSortItems employeeSortItems)
        {
            IQueryable<Employee> employees = GetAllSortedWithouToList(sort);

            if (!string.IsNullOrEmpty(employeeSortItems.FullName))
            {
                employees = employees.Where(e => e.FullName.ToLower()
                .StartsWith(employeeSortItems.FullName));
            }
            if (!string.IsNullOrEmpty(employeeSortItems.Subdivision))
            {
                employees = employees.Where(e => e.Subdivision.ToLower()
                .StartsWith(employeeSortItems.Subdivision));
            }
            if (!string.IsNullOrEmpty(employeeSortItems.Position))
            {
                employees = employees.Where(e => e.Position.ToLower()
                .StartsWith(employeeSortItems.Position));
            }
            if (!string.IsNullOrEmpty(employeeSortItems.Status))
            {
                employees = employees.Where(e => e.Status.ToLower()
                .StartsWith(employeeSortItems.Status));
            }
            if (employeeSortItems.PeoplePartnerId.HasValue)
            {
                employees = employees.Where(e => e.PeoplePartnerId == employeeSortItems.PeoplePartnerId);
            }
            if (employeeSortItems.OutOfOfficeBalance.HasValue)
            {
                employees = employees.Where(e => e.OutOfOfficeBalance == employeeSortItems.OutOfOfficeBalance);
            }

            return await employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.Where(e => e.ID == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
