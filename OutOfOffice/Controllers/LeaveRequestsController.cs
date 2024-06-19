using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Application.Dto.LeaveRequests;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.Services;
using OutOfOffice.Application.SortClasses;
using OutOfOffice.Core.Entities;
using System;
using System.Security.Claims;

namespace OutOfOffice.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;

        public LeaveRequestsController(ILeaveRequestService leaveRequestsService,
            IEmployeeService employeeService,
            UserManager<Employee> userManager,
            IMapper mapper)
        {
            _leaveRequestService = leaveRequestsService;
            _employeeService = employeeService;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sort, LeaveRequestSortItems leaveRequestSortItems)
        {
            var curUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var curEmployee = await _employeeService.GetByIdAsync(curUserId);

            var leaveRequests = await _leaveRequestService.GetAllSortedAndFilteredAsync(sort, leaveRequestSortItems);

            if (curEmployee.Position == Core.Enums.Position.Employee)
            {
                leaveRequests = _leaveRequestService.GetAllForCurrentEmployee(leaveRequests, curUserId);
            }

            ViewData["IDSortParm"] = sort == "ID" ? "ID_desc" : "ID";
            ViewData["EmployeeIdSortParm"] = sort == "EmployeeId" ? "employeeId_desc" : "EmployeeId";
            ViewData["AbsenceReasonSortParm"] = string.IsNullOrEmpty(sort) ? "absenceReason_desc" : "";
            ViewData["StartDateSortParm"] = sort == "StartDate" ? "startDate_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sort == "EndDate" ? "endDate_desc" : "EndDate";
            ViewData["StatusSortParm"] = sort == "Status" ? "status_desc" : "Status";

            ViewData["IDFilter"] = leaveRequestSortItems.ID;
            ViewData["EmployeeIdFilter"] = leaveRequestSortItems.EmployeeId;
            ViewData["AbsenceReasonFilter"] = leaveRequestSortItems.AbsenceReason;
            ViewData["StartDateFilter"] = leaveRequestSortItems.StartDate?.ToString("yyyy-MM-dd");
            ViewData["EndDateFilter"] = leaveRequestSortItems.EndDate?.ToString("yyyy-MM-dd");
            ViewData["StatusFilter"] = leaveRequestSortItems.Status;

            var leaveRequestsIndexDto = _mapper.Map<List<LeaveRequestIndexDto>>(leaveRequests);

            return View(leaveRequestsIndexDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestCreateDto leaveRequestCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(leaveRequestCreateDto);
            }

            try
            {
                var curUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                leaveRequestCreateDto.EmployeeId = curUserId;

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestCreateDto);

                await _leaveRequestService.CreateAsync(leaveRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(leaveRequestCreateDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var leaveRequest = await _leaveRequestService.GetByIdAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            var leaveRequestUpdateDto = _mapper.Map<LeaveRequestUpdateDto>(leaveRequest);
            return View(leaveRequestUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LeaveRequestUpdateDto leaveRequestUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestUpdateDto);
                await _leaveRequestService.UpdateAsync(leaveRequest);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequestUpdateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var leaveRequest = await _leaveRequestService.GetByIdAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            var leaveRequestsIndexDto = _mapper.Map<LeaveRequestIndexDto>(leaveRequest);

            return View(leaveRequestsIndexDto);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id)
        {
            try
            {
                await _leaveRequestService.SubmitAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _leaveRequestService.CancelAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
