using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.Dto.Employees;
using OutOfOffice.Application.IServices;
using OutOfOffice.Core.Entities;
using OutOfOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IEmployeeService _employeeService;

        public AccountService(UserManager<Employee> userManager, IEmployeeService employeeService)
        {
            _userManager = userManager;
            _employeeService = employeeService;
        }

        public async Task CreateAsync(Employee employee, string password)
        {
            await _userManager.CreateAsync(employee, password);
            string role;
            switch (employee.Position)
            {
                case Core.Enums.Position.HRManager:
                    role = "HR Manager";
                    break;
                case Core.Enums.Position.ProjectManager:
                    role = "Project Manager";
                    break;
                case Core.Enums.Position.Administrator:
                    role = "Administrator";
                    break;
                default:
                    role = "Employee";
                    break;
            }
            await _userManager.AddToRoleAsync(employee, role);
        }

        public async Task UpdateAsync(Employee employee)
        {
            employee.SecurityStamp = Guid.NewGuid().ToString();
            // Update the employee user
            await _userManager.UpdateAsync(employee);

            // Determine the role based on the employee's position
            string role;
            switch (employee.Position)
            {
                case Core.Enums.Position.HRManager:
                    role = "HR Manager";
                    break;
                case Core.Enums.Position.ProjectManager:
                    role = "Project Manager";
                    break;
                case Core.Enums.Position.Administrator:
                    role = "Administrator";
                    break;
                default:
                    role = "Employee";
                    break;
            }

            // Remove current roles and assign the new role
            var currentRoles = await _userManager.GetRolesAsync(employee);
            await _userManager.RemoveFromRolesAsync(employee, currentRoles);
            await _userManager.AddToRoleAsync(employee, role);
        }

    }
}
