using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application.Dto.ApprovalRequest;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;
using System.Security.Claims;

namespace OutOfOfficeWeb.Controllers
{
    public class ApprovalRequestController : Controller
    {
        private readonly IApprovalRequestService _approvalRequestService;
        private readonly IMapper _mapper;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService, IMapper mapper)
        {
            _approvalRequestService = approvalRequestService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sort, ApprovalRequestSortItems approvalRequestSortItems)
        {
            var approvalRequests = await _approvalRequestService.GetAllSortedAndFilteredAsync(sort, approvalRequestSortItems);

            ViewData["IDSortParm"] = sort == "ID" ? "ID_desc" : "ID";
            ViewData["ApproverIdSortParm"] = sort == "ApproverId" ? "approverId_desc" : "ApproverId";
            ViewData["LeaveRequestIdSortParm"] = sort == "LeaveRequestId" ? "leaveRequestId_desc" : "LeaveRequestId";
            ViewData["StatusSortParm"] = sort == "Status" ? "status_desc" : "Status";

            ViewData["ApproverIdFilter"] = approvalRequestSortItems.ApproverId;
            ViewData["LeaveRequestIdFilter"] = approvalRequestSortItems.LeaveRequestId;
            ViewData["StatusFilter"] = approvalRequestSortItems.Status;

            var approvalRequestsIndexDto = _mapper.Map<List<ApprovalRequestIndexDto>>(approvalRequests);

            return View(approvalRequestsIndexDto);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var curUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _approvalRequestService.ApproveAsync(id, curUserId);
                TempData["SuccessMessage"] = "Leave request approved successfully.";
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string rejectionComment)
        {
            try
            {
                var curUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _approvalRequestService.RejectAsync(id, rejectionComment, curUserId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error rejecting request: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var approvalRequest = await _approvalRequestService.GetByIdAsync(id);

            if (approvalRequest == null)
            {
                return NotFound();
            }

            return View(approvalRequest);
        }
    }
}
