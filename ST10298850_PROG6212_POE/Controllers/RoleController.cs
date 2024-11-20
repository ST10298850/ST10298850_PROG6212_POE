using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ST10298850_PROG6212_POE.Data;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class RoleController : Controller
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string storedHash, string inputPassword)
        {
            var inputHash = HashPassword(inputPassword);
            return storedHash == inputHash;
        }
        [HttpPost]
        public IActionResult SelectRole(string role, string password, int? lecturerId, int? academicManagerId, int? coordinatorId, int? hrId)
        {
            if (role == "Lecturer" && lecturerId.HasValue)
            {
                var lecturer = _context.Lecturers.SingleOrDefault(l => l.LecturerId == lecturerId.Value);
                if (lecturer != null && VerifyPassword(lecturer.PasswordHash, password))
                {
                    HttpContext.Session.SetInt32("LecturerId", lecturerId.Value);
                    return RedirectToAction("ClaimPageView", "Claim");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid ID or password.");
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (role == "AcademicManager" && academicManagerId.HasValue)
            {
                var manager = _context.AcademicManagers.SingleOrDefault(m => m.ManagerId == academicManagerId.Value);
                if (manager != null && VerifyPassword(manager.PasswordHash, password))
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
                var coordinator = _context.Coordinators.SingleOrDefault(c => c.CoordinatorId == coordinatorId.Value);
                if (coordinator != null && VerifyPassword(coordinator.PasswordHash, password))
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
            else if (role == "HR" && hrId.HasValue)
            {
                var hr = _context.HRs.SingleOrDefault(h => h.hrId == hrId.Value);
                if (hr != null && VerifyPassword(hr.PasswordHash, password))
                {
                    HttpContext.Session.SetInt32("HRId", hrId.Value);
                    return RedirectToAction("HRPageview", "HR");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid HR ID.");
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
