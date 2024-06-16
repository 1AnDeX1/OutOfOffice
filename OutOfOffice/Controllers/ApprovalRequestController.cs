using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Application.Dto.ApprovalRequest;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.SortClasses;

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
    }
}
