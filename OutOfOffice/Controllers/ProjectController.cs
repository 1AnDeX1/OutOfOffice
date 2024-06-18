using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Application.Dto.Projects;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.Services;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;

namespace OutOfOffice.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sort, ProjectSortItems projectSortItems)
        {
            var projects = await _projectService.GetAllSortedAndFilteredAsync(sort, projectSortItems);

            ViewData["IDSortParm"] = sort == "ID" ? "ID_desc" : "ID";
            ViewData["ProjectManagerIdParm"] = sort == "ProjectManagerId" ? "projectmanagerid_desc" : "ProjectManagerId";
            ViewData["ProjectTypeSortParm"] = sort == "ProjectType" ? "projectType_desc" : "ProjectType";
            ViewData["StartDateSortParm"] = sort == "StartDate" ? "startDate_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sort == "EndDate" ? "endDate_desc" : "EndDate";
            ViewData["StatusSortParm"] = sort == "Status" ? "status_desc" : "Status";

            ViewData["IDFilter"] = projectSortItems.ID;
            ViewData["ProjectManagerIdFilter"] = projectSortItems.ProjectManagerId;
            ViewData["ProjectTypeFilter"] = projectSortItems.ProjectType;
            ViewData["StartDateFilter"] = projectSortItems.StartDate?.ToString("yyyy-MM-dd");
            ViewData["EndDateFilter"] = projectSortItems.EndDate?.ToString("yyyy-MM-dd");
            ViewData["StatusFilter"] = projectSortItems.Status;

            var projectsIndexDto = _mapper.Map<List<ProjectIndexDto>>(projects);

            return View(projectsIndexDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateDto projectCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(projectCreateDto);
            }

            try
            {
                var project = _mapper.Map<Project>(projectCreateDto);
                await _projectService.CreateAsync(project);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(projectCreateDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var projectUpdateDto = _mapper.Map<ProjectUpdateDto>(project);
            return View(projectUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProjectUpdateDto projectUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var project = _mapper.Map<Project>(projectUpdateDto);
                await _projectService.UpdateAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(projectUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            await _projectService.DeleteAsync(project);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var projectIndexDto = _mapper.Map<ProjectIndexDto>(project);

            return View(projectIndexDto);
        }
    }

}
