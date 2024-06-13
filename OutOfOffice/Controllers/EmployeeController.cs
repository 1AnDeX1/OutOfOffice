using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;

namespace OutOfOfficeWeb.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService) 
        {
            _employeeService = employeeService;
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

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        
        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            await _employeeService.DeleteAsync(employee);

            return Json(new { success = true });
        }
    }
}
