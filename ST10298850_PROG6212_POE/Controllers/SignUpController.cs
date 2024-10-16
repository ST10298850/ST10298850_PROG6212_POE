using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ILogger<SignUpController> _logger;
        private readonly AppDbContext _context;

        public SignUpController(ILogger<SignUpController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View("SignUpView");
        }

        [HttpPost]
        public IActionResult SignUp(string role, string name, string email, string department)
        {
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Role, Name, and Email are required.");
                return View("SignUpView");
            }

            if (role == "Lecturer")
            {
                var lecturer = new LecturerModel
                {
                    Name = name,
                    Email = email,
                    Department = department
                };
                _context.Lecturers.Add(lecturer);
            }
            else if (role == "AcademicManager")
            {
                var manager = new AcademicManagerModel
                {
                    Name = name,
                    Email = email,
                    Department = department,
                };
                _context.AcademicManagers.Add(manager);
            }
            else if (role == "Coordinator")
            {
                var coordinator = new CoordinatorModel
                {
                    Name = name,
                    Email = email,
                    Department = department,
                };
                _context.Coordinators.Add(coordinator);
            }
            else
            {
                ModelState.AddModelError("", "Invalid role selected.");
                return View("SignUpView");
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
