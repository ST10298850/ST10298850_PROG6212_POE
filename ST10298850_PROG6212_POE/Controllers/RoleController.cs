using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ST10298850_PROG6212_POE.Data;
using System.Linq;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class RoleController : Controller
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SelectRole(string role, int? lecturerId, int? academicManagerId, int? coordinatorId)
        {
            if (role == "Lecturer" && lecturerId.HasValue)
            {
                var lecturerExists = _context.Lecturers.Any(l => l.LecturerId == lecturerId.Value);
                if (lecturerExists)
                {
                    HttpContext.Session.SetInt32("LecturerId", lecturerId.Value);
                    return RedirectToAction("ClaimPageView", "Claim");
                }
                else
                {
                    // Lecturer ID not found in the database
                    ModelState.AddModelError("", "Invalid Lecturer ID.");
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (role == "AcademicManager" && academicManagerId.HasValue)
            {
                var managerExists = _context.AcademicManagers.Any(m => m.ManagerId == academicManagerId.Value);
                if (managerExists)
                {
                    HttpContext.Session.SetInt32("AcademicManagerId", academicManagerId.Value);
                    return RedirectToAction("ApprovalPageView", "Approval");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Manager ID.");
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (role == "Coordinator" && coordinatorId.HasValue)
            {
                var coordinatorExists = _context.Coordinators.Any(c => c.CoordinatorId == coordinatorId.Value);
                if (coordinatorExists)
                {
                    HttpContext.Session.SetInt32("CoordinatorId", coordinatorId.Value);
                    return RedirectToAction("VerificationPageView", "Verification");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Coordinator ID.");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid role or ID.");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
