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

    }
}