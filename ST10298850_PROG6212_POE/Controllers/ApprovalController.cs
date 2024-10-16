﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Data;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly AppDbContext _context; // Assuming this is your data context

        public ApprovalController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApprovalPageView()
        {
            // Fetch the claims that are verified
            var verifiedClaims = _context.Claims
                .Where(c => c.Status == "verified") // Adjust to match your model's property
                .Include(c => c.Documents) // Include related documents
                .Include(c => c.Coordinator) // Include related coordinator
                .Include(c => c.Lecturer) // Include related lecturer
                .ToList();

            return View(verifiedClaims); // Pass the claims to the view
        }

        [HttpGet]
        public IActionResult GetClaimDetailsApproved(int claimId)
        {
            var claim = _context.Claims
                .Include(c => c.Documents)
                .Include(c => c.Coordinator) // Include related coordinator
                .Include(c => c.Lecturer) // Include related lecturer
                .FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null)
            {
                return NotFound();
            }

            // Check for null references for the coordinator and lecturer
            var coordinator = claim.Coordinator; // Get the coordinator for easier access
            var lecturer = claim.Lecturer; // Get the lecturer for easier access

            var claimDetails = new
            {
                LecturerId = lecturer?.LecturerId, // Use null conditional operator
                FullName = lecturer?.Name ?? "NA", // Provide fallback if null
                HourlyRate = claim.HourlyRate.ToString("C"),
                Department = lecturer?.Department ?? "NA", // Provide fallback if null
                Campus = lecturer?.Campus ?? "NA", // Provide fallback if null
                HoursWorked = claim.HoursWorked,
                OvertimeWorked = claim.OvertimeWorked,
                TotalHours = claim.HoursWorked + claim.OvertimeWorked,
                RegularPay = (claim.HoursWorked * claim.HourlyRate).ToString("C"),
                OvertimePay = (claim.OvertimeWorked * (claim.HourlyRate * 1.5M)).ToString("C"),
                TotalPay = ((claim.HoursWorked * claim.HourlyRate) + (claim.OvertimeWorked * (claim.HourlyRate * 1.5M))).ToString("C"),
                CoordinatorId = coordinator?.CoordinatorId, // Use null conditional operator
                CoordinatorName = coordinator?.Name ?? "NA", // Provide fallback if null
                VerificationDate = coordinator?.VerificationDate.ToString() ?? "NA" // Provide fallback if null
            };

            return Json(claimDetails);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "approved"; // Set the status to verified
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
