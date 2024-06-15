using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OutOfOffice.Application.Dto.Employees;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;

namespace OutOfOfficeWeb.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sort, EmployeeSortItems employeeSortItems)
        {
            var employees = await _employeeService.GetAllSortedAndFilteredAsync(sort, employeeSortItems);

            ViewData["NameSortParm"] = string.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewData["SubdivisionSortParm"] = sort == "Subdivision" ? "subdivision_desc" : "Subdivision";
            ViewData["PositionSortParm"] = sort == "Position" ? "position_desc" : "Position";
            ViewData["StatusSortParm"] = sort == "Status" ? "status_desc" : "Status";
            ViewData["PeoplePartnerSortParm"] = sort == "PeoplePartner" ? "peoplepartner_desc" : "PeoplePartner";
            ViewData["BalanceSortParm"] = sort == "Balance" ? "balance_desc" : "Balance";

            ViewData["FullNameFilter"] = employeeSortItems.FullName;
            ViewData["SubdivisionFilter"] = employeeSortItems.Subdivision;
            ViewData["PositionFilter"] = employeeSortItems.Position;
            ViewData["StatusFilter"] = employeeSortItems.Status;
            ViewData["PeoplePartnerFilter"] = employeeSortItems.PeoplePartnerId;
            ViewData["BalanceFilter"] = employeeSortItems.OutOfOfficeBalance;

            var employeesIndexDto = _mapper.Map<List<EmployeeIndexDto>>(employees);

            return View(employeesIndexDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeIndexDto employeeIndexDto)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeeIndexDto);
                await _employeeService.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employeeIndexDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeIndexDto = _mapper.Map<EmployeeIndexDto>(employee);
            return View(employeeIndexDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeIndexDto employeeIndexDto)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeeIndexDto);
                await _employeeService.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employeeIndexDto);
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _employeeService.DeleteAsync(employee);
            return Json(new { success = true });
        }
    }

}
