using Microsoft.AspNetCore.Mvc;

namespace OutOfOfficeWeb.Controllers
{
    public class ApprovalRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
