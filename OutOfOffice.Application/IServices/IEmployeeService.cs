using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.IServices
{
    public interface IEmployeeService
    {
        Task<IList<Employee>> GetAllAsync();

        Task<IList<Employee>> GetAllSortedAsync(string sort);

        Task<IList<Employee>> GetAllSortedAndFilteredAsync(string sort, EmployeeSortItems employeeSortItems);

        Task AddAsync(Employee employee); 

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(Employee employee);

        Task<Employee?> GetByIdAsync(int id);

        Task<bool> EmployeeExistsAsync(int? employeeId);
    }
}
