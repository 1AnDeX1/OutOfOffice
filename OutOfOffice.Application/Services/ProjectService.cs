﻿using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.Dto.Employees;
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

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectEmployeeDto>> GetAssignedEmployeesAsync(int projectId)
        {
            var assignedEmployees = await _context.ProjectAssignments
                                                  .Where(pa => pa.ProjectId == projectId)
                                                  .Select(pa => new ProjectEmployeeDto
                                                  {
                                                      Id = pa.EmployeeId,
                                                      FullName = pa.Employee.FullName
                                                  })
                                                  .ToListAsync();
            return assignedEmployees;
        }

        public async Task AssignEmployeeAsync(int projectId, int employeeId)
        {
            // Check if the assignment already exists
            var existingAssignment = await _context.ProjectAssignments
                                                   .FirstOrDefaultAsync(pa => pa.ProjectId == projectId && pa.EmployeeId == employeeId);

            if (existingAssignment == null)
            {
                // Create a new assignment
                var newAssignment = new ProjectAssignment
                {
                    ProjectId = projectId,
                    EmployeeId = employeeId
                };

                _context.ProjectAssignments.Add(newAssignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveEmployeeAsync(int projectId, int employeeId)
        {
            var assignmentToRemove = await _context.ProjectAssignments
                                                    .FirstOrDefaultAsync(pa => pa.ProjectId == projectId && pa.EmployeeId == employeeId);

            if (assignmentToRemove != null)
            {
                _context.ProjectAssignments.Remove(assignmentToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }

}
