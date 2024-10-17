using Microsoft.AspNetCore.Mvc;
using ST10298850_PROG6212_POE.Models;
using ST10298850_PROG6212_POE.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


//[HttpGet]
//public IActionResult SignUp()
//{
//    return View("SignUpView");
//}

//[HttpPost]
//public IActionResult SignUp(string role, string name, string email, string department)
//{
//    if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
//    {
//        ModelState.AddModelError("", "Role, Name, and Email are required.");
//        return View("SignUpView");
//    }

//    if (role == "Lecturer")
//    {
//        var lecturer = new LecturerModel
//        {
//            Name = name,
//            Email = email,
//            Department = department
//        };
//        _context.Lecturers.Add(lecturer);
//    }
//    else if (role == "AcademicManager")
//    {
//        var manager = new AcademicManagerModel
//        {
//            Name = name,
//            Email = email,
//            Department = department,

//        };
//        _context.AcademicManagers.Add(manager);
//    }
//    else if (role == "Coordinator")
//    {
//        var coordinator = new CoordinatorModel
//        {
//            Name = name,
//            Email = email,
//            Department = department,

//        };
//        _context.Coordinators.Add(coordinator);
//    }
//    else
//    {
//        ModelState.AddModelError("", "Invalid role selected.");
//        return View("SignUpView");
//    }

//    _context.SaveChanges();
//    return RedirectToAction("Index");
//}

//[HttpPost]
//public IActionResult SelectRole(string role, int? lecturerId)
//{
//    if (role == "Lecturer" && lecturerId.HasValue)
//    {
//        // Store the LecturerID in the session
//        HttpContext.Session.SetInt32("LecturerId", lecturerId.Value);
//        return RedirectToAction("ClaimPageView");
//    }
//    else if (role == "AcademicManager")
//    {
//        return RedirectToAction("AcademicManagerPageView");
//    }
//    else if (role == "Coordinator")
//    {
//        return RedirectToAction("CoordinatorPageView");
//    }
//    else
//    {
//        // Handle invalid selection
//        return RedirectToAction("Index");
//    }
//}



        //public IActionResult ClaimPageView()
        //{
        //    // Retrieve the LecturerID from the session
        //    int? lecturerId = HttpContext.Session.GetInt32("LecturerId");
        //    if (lecturerId.HasValue)
        //    {
        //        // Pass the lecturerId to the view
        //        ViewBag.LecturerId = lecturerId.Value;
        //    }
        //    else
        //    {
        //        // Handle the case where the lecturerId is not found in the session
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult SubmitClaim(int lecturerId, decimal hoursWorked, decimal overtimeWorked, string documentName, IFormFile documentFile)
        //{
        //    // Log the start of the method
        //    _logger.LogInformation("SubmitClaim method started.");

        //    // Check if lecturer exists
        //    var lecturer = _context.Lecturers?.FirstOrDefault(l => l.LecturerId == lecturerId);
        //    if (lecturer == null)
        //    {
        //        _logger.LogWarning("Lecturer not found: {LecturerId}", lecturerId);
        //        return NotFound("Lecturer not found");
        //    }

        //    // Create a new claim
        //    var newClaim = new LecturerClaimModel
        //    {
        //        LecturerId = lecturerId,
        //        HoursWorked = hoursWorked,
        //        OvertimeWorked = overtimeWorked,
        //        SubmissionDate = DateTime.Now,
        //        Documents = new List<DocumentModel>(),
        //    };

        //    // Save the document file
        //    if (documentFile != null && documentFile.Length > 0)
        //    {
        //        var filePath = Path.Combine("wwwroot/documents", documentFile.FileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            documentFile.CopyTo(stream);
        //        }

        //        // Create a document
        //        var newDocument = new DocumentModel
        //        {
        //            DocumentName = documentName,
        //            Claim = newClaim,
        //            FilePath = filePath
        //        };

        //        // Add document to the claim
        //        newClaim.Documents.Add(newDocument);
        //    }

        //    // Check model state
        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogWarning("Model state is invalid.");
        //        return View("ClaimPageView");
        //    }

        //    // Save to the database
        //    try
        //    {
        //        _context.Claims?.Add(newClaim);
        //        _context.SaveChanges();
        //        _logger.LogInformation("Claim saved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving claim.");
        //        return StatusCode(500, "Internal server error");
        //    }

        //    return RedirectToAction("ClaimPageView");
        //}

        //public IActionResult VerificationPageView()
        //{
        //    return View();
        //}

        //public IActionResult ApprovalPageView()
        //{
        //    return View();
        //}

