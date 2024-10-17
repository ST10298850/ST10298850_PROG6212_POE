using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models; // Adjust this according to your models namespace
using System.Linq;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class VerificationController : Controller
    {
        private readonly AppDbContext _context;

        public VerificationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult VerificationPageView()
        {
            // Fetch all claims with status "Pending" from the database and include related Documents
            var claims = _context.Claims
                                 .Where(c => c.Status == "Pending")
                                 .Include(c => c.Documents)
                                 .ToList();

            // Pass the claims to the view
            return View(claims);
        }

        [HttpGet]
        public IActionResult GetClaimDetails(int id)
        {
            // Fetch the claim with the given ID
            var claim = _context.Claims
                .Include(c => c.Lecturer) // Include related lecturer
                .FirstOrDefault(c => c.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            // Prepare data to return in JSON format
            var claimDetails = new
            {
                LecturerId = claim.Lecturer.LecturerId,
                FullName = claim.Lecturer.Name,
                HourlyRate = claim.HourlyRate.ToString("C"), // Format as currency
                Department = claim.Lecturer.Department,
                Campus = claim.Lecturer.Campus,
                HoursWorked = claim.HoursWorked,
                OvertimeWorked = claim.OvertimeWorked,
                TotalHours = claim.HoursWorked + claim.OvertimeWorked,
                RegularPay = (claim.HoursWorked * claim.HourlyRate).ToString("C"),
                OvertimePay = (claim.OvertimeWorked * (claim.HourlyRate * 1.5M)).ToString("C"), // Assume 1.5x for overtime
                TotalPay = ((claim.HoursWorked * claim.HourlyRate) + (claim.OvertimeWorked * (claim.HourlyRate * 1.5M))).ToString("C")
            };

            // Return the data as JSON
            return Json(claimDetails);
        }
        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "verified"; // Set the status to verified
                _context.SaveChanges(); // Save changes to the database
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult RejectClaim(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "rejected"; // Set the status to rejected
                _context.SaveChanges(); // Save changes to the database
                return Ok();
            }
            return NotFound();
        }
    }
}
