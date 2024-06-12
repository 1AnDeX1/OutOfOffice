using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.IServices;
using OutOfOffice.Core.Entities;
using OutOfOffice.Infrastructure.Data;
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

        public async Task<IList<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }
    }
}
