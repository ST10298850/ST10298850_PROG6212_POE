using Microsoft.AspNetCore.Mvc;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;
using System.Text;
using System.Security.Cryptography;
using System.Text;

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
    // Helper method for hashing passwords
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    [HttpPost]
    public IActionResult SignUp(string role, string name, string email, string department, string campus, string managerDepartment, string coordinatorDepartment, string password)
    {
        if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Role, Name, and Email are required.");
            return View("SignUpView");
        }

        int userId = 0;

        string hashedPassword = HashPassword(password);//Passwrod attribute is hashed

        if (role == "Lecturer")
        {
            var lecturer = new LecturerModel
            {
                Name = name,
                Email = email,
                Department = department,  // Uses the general 'department' for Lecturer
                Campus = campus,
                 PasswordHash = hashedPassword
            };
            _context.Lecturers.Add(lecturer);
            _context.SaveChanges();

            userId = lecturer.LecturerId;
            HttpContext.Session.SetInt32("LecturerID", userId); // Store LecturerId in session
        }
        else if (role == "AcademicManager")
        {
            var manager = new AcademicManagerModel
            {
                Name = name,
                Email = email,
                Department = managerDepartment,  // Uses manager-specific department
                 PasswordHash = hashedPassword
            };
            _context.AcademicManagers.Add(manager);
            _context.SaveChanges();

            userId = manager.ManagerId;
            HttpContext.Session.SetInt32("ManagerID", userId); // Store ManagerId in session
        }
        else if (role == "Coordinator")
        {
            var coordinator = new CoordinatorModel
            {
                Name = name,
                Email = email,
                Department = coordinatorDepartment,  // Uses coordinator-specific department
                 PasswordHash = hashedPassword
            };
            _context.Coordinators.Add(coordinator);
            _context.SaveChanges();

            userId = coordinator.CoordinatorId;
            HttpContext.Session.SetInt32("CoordinatorID", userId); // Store CoordinatorId in session
        }
        else if (role == "HR")
        {
            var hr = new HRModel
            {
                Name = name,
                Email = email,
                PasswordHash = hashedPassword
            };
            _context.HRs.Add(hr);
            _context.SaveChanges();

            userId = hr.hrId;
            HttpContext.Session.SetInt32("hrId", userId); // Store CoordinatorId in session
        }
        else
        {
            ModelState.AddModelError("", "Invalid role selected.");
            return View("SignUpView");
        }

        ViewBag.UserId = userId;
        return View("SignUpView");
    }

}