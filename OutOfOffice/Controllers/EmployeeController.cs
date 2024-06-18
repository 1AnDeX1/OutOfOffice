using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public EmployeeController(IEmployeeService employeeService,
            IMapper mapper,
            UserManager<Employee> userManager,
            IAccountService accountService)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _userManager = userManager;
            _accountService = accountService;
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
        public async Task<IActionResult> Create(EmployeeCreateDto employeeCreateDto)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeeCreateDto);
                employee.UserName = employeeCreateDto.Email;

                await _accountService.CreateAsync(employee, employeeCreateDto.Password);

                return RedirectToAction(nameof(Index));

            }
            return View(employeeCreateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeUpdateDto = _mapper.Map<EmployeeUpdateDto>(employee);
            return View(employeeUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeUpdateDto employeeUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var employee = await _employeeService.GetByIdAsync(employeeUpdateDto.ID);
                
                _mapper.Map(employeeUpdateDto, employee);

                await _accountService.UpdateAsync(employee);

                return RedirectToAction(nameof(Index));
            }

            return View(employeeUpdateDto);
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

            var currentRoles = await _userManager.GetRolesAsync(employee);
            await _userManager.RemoveFromRolesAsync(employee, currentRoles);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeDetailsDto = _mapper.Map<EmployeeDetailsDto>(employee);
            return View(employeeDetailsDto);
        }
    }

}
