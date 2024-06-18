using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application.IServices
{
    public interface IProjectService
    {
        Task<IList<Project>> GetAllSortedAsync(string sort);
        Task<IList<Project>> GetAllSortedAndFilteredAsync(string sort, ProjectSortItems projectSortItems);
        Task<Project> GetByIdAsync(int id);
        Task CreateAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
