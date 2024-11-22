using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ApprovalController> _logger;

        // Constructor to initialize the database context and logger
        public ApprovalController(AppDbContext context, ILogger<ApprovalController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Default action to return the index view
        public IActionResult Index()
        {
            return View();
        }

        // Action to display the approval page with verified claims
        public IActionResult ApprovalPageView()
        {
            try
            {
                // Fetch claims with status "verified" and include related entities
                var verifiedClaims = _context.Claims
                    .Where(c => c.Status == "verified")
                    .Include(c => c.Documents)
                    .Include(c => c.Coordinator)
                    .Include(c => c.Lecturer)
                    .ToList();

                // Pass the verified claims to the view
                return View(verifiedClaims);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 status code
                _logger.LogError(ex, "Error occurred while fetching verified claims.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // Action to get details of an approved claim by its ID
        [HttpGet]
        public IActionResult GetClaimDetailsApproved(int claimId)
        {
            try
            {
                // Fetch the claim by ID and include related entities
                var claim = _context.Claims
                    .Include(c => c.Documents)
                    .Include(c => c.Coordinator)
                    .Include(c => c.Lecturer)
                    .FirstOrDefault(c => c.ClaimId == claimId);

                // If claim not found, return 404 status code
                if (claim == null)
                {
                    return NotFound("Claim not found.");
                }

                // Validate the claim details
                bool isValidHourlyRate = IsClaimValid(claim).Item1;
                bool isValidHoursWorked = IsClaimValid(claim).Item2;
                bool isValidOvertimeWorked = IsClaimValid(claim).Item3;

                // Prepare claim details for the response
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
                    CoordinatorId = coordinator?.CoordinatorId,
                    CoordinatorName = coordinator?.Name ?? "NA",
                    VerificationDate = coordinator?.VerificationDate.ToString() ?? "NA",
                    isValidHourlyRate,
                    isValidHoursWorked,
                    isValidOvertimeWorked
                };

                // Return the claim details as JSON
                return Json(claimDetails);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 status code
                _logger.LogError(ex, "Error occurred while fetching claim details.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            try
            {
                var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);
                if (claim != null)
                {
                    claim.Status = "approved";
                    _context.SaveChanges();
                    return Ok();
                }
                return NotFound("Claim not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while approving claim.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        public IActionResult RejectClaim(int id)
        {
            try
            {
                var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);
                if (claim != null)
                {
                    claim.Status = "rejected";
                    _context.SaveChanges();
                    return Ok();
                }
                return NotFound("Claim not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rejecting claim.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        // Helper method to validate claim details
        private (bool isHourlyRateValid, bool isHoursWorkedValid, bool isOvertimeValid) IsClaimValid(LecturerClaimModel claim)
        {
            // Validate hourly rate (50 - 400)
            bool isHourlyRateValid = claim.HourlyRate >= 50 && claim.HourlyRate <= 400;

            // Validate hours worked (35 - 80)
            bool isHoursWorkedValid = claim.HoursWorked >= 35 && claim.HoursWorked <= 80;

            // Validate overtime worked (cannot exceed 10 hours)
            bool isOvertimeValid = claim.OvertimeWorked <= 10;

            // Return validation results as a tuple
            return (isHourlyRateValid, isHoursWorkedValid, isOvertimeValid);
        }
    }
}
