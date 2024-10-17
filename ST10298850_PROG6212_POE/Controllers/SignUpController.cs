using Microsoft.AspNetCore.Mvc;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;

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
    public IActionResult SignUp(string role, string name, string email, string department, string campus, string managerDepartment, string coordinatorDepartment)
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
                Department = department,  // Uses the general 'department' for Lecturer
                Campus = campus
            };
            _context.Lecturers.Add(lecturer);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("LecturerID", lecturer.LecturerId); // Store LecturerId in session
        }
        else if (role == "AcademicManager")
        {
            var manager = new AcademicManagerModel
            {
                Name = name,
                Email = email,
                Department = managerDepartment  // Uses manager-specific department
            };
            _context.AcademicManagers.Add(manager);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("ManagerID", manager.ManagerId); // Store ManagerId in session
        }
        else if (role == "Coordinator")
        {
            var coordinator = new CoordinatorModel
            {
                Name = name,
                Email = email,
                Department = coordinatorDepartment  // Uses coordinator-specific department
            };
            _context.Coordinators.Add(coordinator);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("CoordinatorID", coordinator.CoordinatorId); // Store CoordinatorId in session
        }
        else
        {
            ModelState.AddModelError("", "Invalid role selected.");
            return View("SignUpView");
        }

        return RedirectToAction("Index", "Home");
    }
}