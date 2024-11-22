using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class HRController : Controller
    {
        private readonly AppDbContext _context;

        public HRController(AppDbContext context)
        {
            _context = context;
        }

        // Display HR Page
        public async Task<IActionResult> HRPageView(string filterType, string filterValue)
        {
            // Fetch all claims
            var claims = _context.Claims.AsQueryable();

            // Filter claims if filter parameters are provided
            if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterType.ToLower())
                {
                    case "status":
                        claims = claims.Where(c => c.Status.Contains(filterValue));
                        break;
                    case "userid":
                        if (int.TryParse(filterValue, out var userId))
                            claims = claims.Where(c => c.LecturerId == userId);
                        break;
                }
            }

            // Fetch all users
            var users = await _context.Lecturers.ToListAsync();

            // Pass data to view
            ViewBag.Claims = await claims.Include(c => c.Lecturer).ToListAsync();
            return View(users);
        }



        // Update User Information
        [HttpPost]
        public async Task<IActionResult> UpdateUser(LecturerModel updatedUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Lecturers.FindAsync(updatedUser.LecturerId);
                if (user != null)
                {
                    user.Name = updatedUser.Name;
                    user.Email = updatedUser.Email;
                    // Update other fields as needed
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("HRPageView");
            }

            return RedirectToAction("HRPageView");
        }

        [HttpGet]
        public async Task<IActionResult> GenerateReport(string filterType, string filterValue)
        {
            var claims = _context.Claims.AsQueryable();

            if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterType.ToLower())
                {
                    case "status":
                        claims = claims.Where(c => c.Status == filterValue);
                        break;
                    case "userid":
                        if (int.TryParse(filterValue, out var userId))
                            claims = claims.Where(c => c.LecturerId == userId);
                        break;
                    default:
                        return BadRequest("Invalid filter type.");
                }
            }

            var filteredClaims = await claims
                .Include(c => c.Lecturer)
                .Include(c => c.Coordinator)
                .Include(c => c.Documents)
                .ToListAsync();

            // Generate CSV
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Claim ID,User ID,Status,Submission Date,Lecturer Name,Hourly Rate,Department,Campus,Hours Worked,Overtime Worked,Total Hours,Regular Pay,Overtime Pay,Total Pay,Notes,Coordinator Name,Verification Date,Documents");

            foreach (var claim in filteredClaims)
            {
                var documents = string.Join(";", claim.Documents.Select(d => d.DocumentName));
                var coordinator = claim.Coordinator;
                var lecturer = claim.Lecturer;

                var claimDetails = new
                {
                    LecturerId = lecturer?.LecturerId,
                    FullName = lecturer?.Name ?? "NA",
                    HourlyRate = claim.HourlyRate.ToString("C"),
                    Department = lecturer?.Department ?? "NA",
                    Campus = lecturer?.Campus ?? "NA",
                    HoursWorked = claim.HoursWorked,
                    OvertimeWorked = claim.OvertimeWorked,
                    TotalHours = claim.HoursWorked + claim.OvertimeWorked,
                    RegularPay = (claim.HoursWorked * claim.HourlyRate).ToString("C"),
                    OvertimePay = (claim.OvertimeWorked * (claim.HourlyRate * 1.5M)).ToString("C"),
                    TotalPay = ((claim.HoursWorked * claim.HourlyRate) + (claim.OvertimeWorked * (claim.HourlyRate * 1.5M))).ToString("C"),
                    Notes = claim.Notes,
                    CoordinatorName = coordinator?.Name ?? "NA",
                    VerificationDate = coordinator?.VerificationDate.ToString() ?? "NA"
                };

                csvContent.AppendLine($"{claim.ClaimId},{claim.LecturerId},{claim.Status},{claim.SubmissionDate:yyyy-MM-dd},{claimDetails.FullName},{claimDetails.HourlyRate},{claimDetails.Department},{claimDetails.Campus},{claimDetails.HoursWorked},{claimDetails.OvertimeWorked},{claimDetails.TotalHours},{claimDetails.RegularPay},{claimDetails.OvertimePay},{claimDetails.TotalPay},{claimDetails.Notes},{claimDetails.CoordinatorName},{claimDetails.VerificationDate},{documents}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            return File(csvBytes, "text/csv", $"ClaimsReport_{DateTime.Now:yyyyMMddHHmmss}.csv");
        }


    }
}