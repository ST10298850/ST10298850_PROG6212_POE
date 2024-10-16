using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class RoleController : Controller
    {
        [HttpPost]
        public IActionResult SelectRole(string role, int? lecturerId)
        {
            if (role == "Lecturer" && lecturerId.HasValue)
            {
                HttpContext.Session.SetInt32("LecturerId", lecturerId.Value);
                return RedirectToAction("ClaimPageView", "Claim");
            }
            else if (role == "AcademicManager")
            {
                return RedirectToAction("ApprovalPageView");
            }
            else if (role == "Coordinator")
            {
                return RedirectToAction("ApprovalPageView");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
