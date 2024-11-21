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
                    case "date":
                        if (DateTime.TryParse(filterValue, out var filterDate))
                            claims = claims.Where(c => c.SubmissionDate.Date == filterDate.Date);
                        break;
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

        public async Task<IActionResult> GenerateReport(string filterType, string filterValue)
        {
            // Fetch and filter claims as in HRPageView
            var claims = _context.Claims.AsQueryable();

            if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterType.ToLower())
                {
                    case "date":
                        if (DateTime.TryParse(filterValue, out var filterDate))
                            claims = claims.Where(c => c.SubmissionDate.Date == filterDate.Date);
                        break;
                    case "status":
                        claims = claims.Where(c => c.Status.Contains(filterValue));
                        break;
                    case "userid":
                        if (int.TryParse(filterValue, out var userId))
                            claims = claims.Where(c => c.LecturerId == userId);
                        break;
                }
            }

            var filteredClaims = await claims.Include(c => c.Lecturer).ToListAsync();

            // Convert claims to CSV
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Claim ID,User ID,Status,Submission Date");

            foreach (var claim in filteredClaims)
            {
                csvContent.AppendLine($"{claim.ClaimId},{claim.LecturerId},{claim.Status},{claim.SubmissionDate:yyyy-MM-dd}");
            }

            // Return as downloadable file
            var csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            var csvFileName = $"ClaimsReport_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(csvBytes, "text/csv", csvFileName);
        }
    }
}