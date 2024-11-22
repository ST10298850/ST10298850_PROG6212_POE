using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class HRController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HRController> _logger;

        public HRController(AppDbContext context, ILogger<HRController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Display HR Page
        public async Task<IActionResult> HRPageView(string filterType, string filterValue)
        {
            try
            {
                // Fetch all claims
                var claims = _context.Claims.AsQueryable(); // Get all claims

                // Filter claims if filter parameters are provided
                if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
                {
                    switch (filterType.ToLower()) // Convert to lower case for to eliminate human error 
                    {
                        case "status":
                            claims = claims.Where(c => c.Status.Contains(filterValue)); // Use Contains to match partial status
                            break;
                        case "userid":
                            if (int.TryParse(filterValue, out var userId)) // Check if user ID is a valid integer
                                claims = claims.Where(c => c.LecturerId == userId); // Filter by user ID
                            break;
                        default:
                            return BadRequest("Invalid filter type.");
                    }
                }

                // Fetch all users
                var users = await _context.Lecturers.ToListAsync(); // Get all users

                // Pass data to view
                ViewBag.Claims = await claims.Include(c => c.Lecturer).ToListAsync(); // Include lecturer details
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching HR page data.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // Update User Information
        [HttpPost]
        public async Task<IActionResult> UpdateUser(int userID, string name, string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Lecturers.FindAsync(userID);
                    if (user != null)
                    {
                        // Update only the Name and Email fields
                        user.Name = name;
                        user.Email = email;
                        await _context.SaveChangesAsync();
                        return Ok(); // Return Ok status
                    }
                    else
                    {
                        return NotFound("User not found.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating user information.");
                    return StatusCode(500, "Internal server error. Please try again later.");
                }
            }

            return BadRequest("Invalid user data.");
        }



        [HttpGet]
        public async Task<IActionResult> GenerateReport(string filterType, string filterValue)
        {
            try
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

                // Generate text report
                var reportContent = new StringBuilder();
                reportContent.AppendLine("Claims Report");
                reportContent.AppendLine("======================================");

                foreach (var claim in filteredClaims)
                {
                    var documents = string.Join(";", claim.Documents.Select(d => d.DocumentName));
                    var coordinator = claim.Coordinator;
                    var lecturer = claim.Lecturer;

                    reportContent.AppendLine($"Claim ID: {claim.ClaimId}");
                    reportContent.AppendLine($"User ID: {claim.LecturerId}");
                    reportContent.AppendLine($"Status: {claim.Status}");
                    reportContent.AppendLine($"Submission Date: {claim.SubmissionDate:yyyy-MM-dd}");
                    reportContent.AppendLine($"Lecturer Name: {lecturer?.Name ?? "NA"}");
                    reportContent.AppendLine($"Hourly Rate: {claim.HourlyRate:C}");
                    reportContent.AppendLine($"Department: {lecturer?.Department ?? "NA"}");
                    reportContent.AppendLine($"Campus: {lecturer?.Campus ?? "NA"}");
                    reportContent.AppendLine($"Hours Worked: {claim.HoursWorked}");
                    reportContent.AppendLine($"Overtime Worked: {claim.OvertimeWorked}");
                    reportContent.AppendLine($"Total Hours: {claim.HoursWorked + claim.OvertimeWorked}");
                    reportContent.AppendLine($"Regular Pay: {(claim.HoursWorked * claim.HourlyRate):C}");
                    reportContent.AppendLine($"Overtime Pay: {(claim.OvertimeWorked * (claim.HourlyRate * 1.5M)):C}");
                    reportContent.AppendLine($"Total Pay: {((claim.HoursWorked * claim.HourlyRate) + (claim.OvertimeWorked * (claim.HourlyRate * 1.5M))):C}");
                    reportContent.AppendLine($"Notes: {claim.Notes}");
                    reportContent.AppendLine($"Coordinator Name: {coordinator?.Name ?? "NA"}");
                    reportContent.AppendLine($"Verification Date: {coordinator?.VerificationDate.ToString() ?? "NA"}");
                    reportContent.AppendLine($"Documents: {documents}");
                    reportContent.AppendLine("======================================");
                }

                var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
                return File(reportBytes, "text/plain", $"ClaimsReport_{DateTime.Now:yyyyMMddHHmmss}.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating report.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GenerateInvoice(int id)
        {
            try
            {
                var claim = await _context.Claims
                    .Include(c => c.Lecturer)
                    .FirstOrDefaultAsync(c => c.ClaimId == id);

                if (claim == null)
                {
                    return NotFound("Claim not found.");
                }

                var lecturer = claim.Lecturer;
                var invoiceContent = new StringBuilder();
                invoiceContent.AppendLine("======================================");
                invoiceContent.AppendLine("               INVOICE                ");
                invoiceContent.AppendLine("======================================");
                invoiceContent.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd}");
                invoiceContent.AppendLine("");
                invoiceContent.AppendLine("Billing Information:");
                invoiceContent.AppendLine($"Lecturer ID: {lecturer?.LecturerId}");
                invoiceContent.AppendLine($"Lecturer Name: {lecturer?.Name}");
                invoiceContent.AppendLine("");
                invoiceContent.AppendLine("Claim Details:");
                invoiceContent.AppendLine($"Claim ID: {claim.ClaimId}");
                invoiceContent.AppendLine($"Submission Date: {claim.SubmissionDate:yyyy-MM-dd}");
                invoiceContent.AppendLine($"Status: {claim.Status}");
                invoiceContent.AppendLine("");
                invoiceContent.AppendLine("Hours and Pay Summary:");
                invoiceContent.AppendLine($"Hourly Rate: {claim.HourlyRate:C}");
                invoiceContent.AppendLine($"Regular Hours: {claim.HoursWorked}");
                invoiceContent.AppendLine($"Overtime Hours: {claim.OvertimeWorked}");
                invoiceContent.AppendLine($"Total Hours: {claim.HoursWorked + claim.OvertimeWorked}");
                invoiceContent.AppendLine($"Regular Pay: {(claim.HoursWorked * claim.HourlyRate):C}");
                invoiceContent.AppendLine($"Overtime Pay: {(claim.OvertimeWorked * (claim.HourlyRate * 1.5M)):C}");
                invoiceContent.AppendLine($"--------------------------------------");
                invoiceContent.AppendLine($"TOTAL PAY: {((claim.HoursWorked * claim.HourlyRate) + (claim.OvertimeWorked * (claim.HourlyRate * 1.5M))):C}");
                invoiceContent.AppendLine("");
                invoiceContent.AppendLine("Notes:");
                invoiceContent.AppendLine(string.IsNullOrWhiteSpace(claim.Notes) ? "No additional notes provided." : claim.Notes);
                invoiceContent.AppendLine("");
                invoiceContent.AppendLine("======================================");

                var invoiceBytes = Encoding.UTF8.GetBytes(invoiceContent.ToString());
                return File(invoiceBytes, "text/plain", $"Invoice_{claim.ClaimId}.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating invoice.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }
}
