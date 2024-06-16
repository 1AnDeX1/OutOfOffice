using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using OutOfOffice.Data;

namespace OutOfOffice.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly OutOfOfficeDbContext _context;
        private readonly IEmployeeService _employeeService;

        public ProjectService(OutOfOfficeDbContext context,
            IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        protected IQueryable<Project> GetAllSortedWithoutToList(string sort)
        {
            IQueryable<Project> projects = _context.Projects;
            projects = sort switch
            {
                "ID" => projects.OrderBy(p => p.ID),
                "ID_desc" => projects.OrderByDescending(p => p.ID),
                "ProjectManagerId" => projects.OrderBy(p => p.ProjectManagerId),
                "projectmanagerid_desc" => projects.OrderByDescending(p => p.ProjectManagerId),
                "ProjectType" => projects.OrderBy(p => p.ProjectType),
                "projectType_desc" => projects.OrderByDescending(p => p.ProjectType),
                "StartDate" => projects.OrderBy(p => p.StartDate),
                "startDate_desc" => projects.OrderByDescending(p => p.StartDate),
                "EndDate" => projects.OrderBy(p => p.EndDate),
                "endDate_desc" => projects.OrderByDescending(p => p.EndDate),
                "Status" => projects.OrderBy(p => p.Status),
                "status_desc" => projects.OrderByDescending(p => p.Status),
                _ => projects.OrderBy(p => p.ProjectType),
            };

            return projects;
        }

        public async Task<IList<Project>> GetAllSortedAsync(string sort)
        {
            var sortedprojects = GetAllSortedWithoutToList(sort);
            return await sortedprojects.ToListAsync();
        }

        public async Task<IList<Project>> GetAllSortedAndFilteredAsync(string sort, ProjectSortItems projectSortItems)
        {
            IQueryable<Project> projects = GetAllSortedWithoutToList(sort);

            if (projectSortItems.ID.HasValue)
            {
                projects = projects.Where(p => p.ID == projectSortItems.ID);
            }
            if (projectSortItems.ProjectType != null)
            {
                projects = projects.Where(p => p.ProjectType == projectSortItems.ProjectType);
            }
            if (projectSortItems.StartDate.HasValue)
            {
                projects = projects.Where(p => p.StartDate >= projectSortItems.StartDate.Value);
            }
            if (projectSortItems.EndDate.HasValue)
            {
                projects = projects.Where(p => p.EndDate <= projectSortItems.EndDate.Value);
            }
            if (projectSortItems.ProjectManagerId.HasValue)
            {
                projects = projects.Where(p => p.ProjectManagerId == projectSortItems.ProjectManagerId.Value);
            }
            if (projectSortItems.Status != null)
            {
                projects = projects.Where(p => p.Status == projectSortItems.Status);
            }

            return await projects.ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task CreateAsync(Project project)
        {
            if (!await _employeeService.EmployeeExistsAsync(project.ProjectManagerId))
            {
                throw new Exception("Employee does not exist.");
            }

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

    }

}
