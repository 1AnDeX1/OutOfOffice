using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.IServices
{
    public interface IAccountService
    {
        
        Task CreateAsync(Employee employee, string password);
        Task UpdateAsync(Employee employee);
    }
}
