using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models; // Adjust this according to your models namespace
using System.Diagnostics;
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
            try
            {
                // Fetch all claims with status "Pending" from the database and include related Documents
                var claims = _context.Claims
                                     .Where(c => c.Status == "Pending")
                                     .Include(c => c.Documents)
                                     .ToList();

                // Pass the claims to the view
                return View(claims);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                Console.WriteLine(ex.Message);
                // Return an error view or message
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public IActionResult GetClaimDetails(int id)
        {
            try
            {
                // Fetch the claim with the given ID
                var claim = _context.Claims
                    .Include(c => c.Lecturer) // Include related lecturer
                    .FirstOrDefault(c => c.ClaimId == id);

                if (claim == null)
                {
                    return NotFound("Claim not found.");
                }

                // Validate the claim
                var validationResults = IsClaimValid(claim);

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
                    TotalPay = ((claim.HoursWorked * claim.HourlyRate) + (claim.OvertimeWorked * (claim.HourlyRate * 1.5M))).ToString("C"),
                    Notes = claim.Notes,
                    // Include validation results
                    isValidHourlyRate = validationResults.isHourlyRateValid,
                    isValidHoursWorked = validationResults.isHoursWorkedValid,
                    isValidOvertimeWorked = validationResults.isOvertimeValid
                };

                // Return the data as JSON
                return Json(claimDetails);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                Console.WriteLine(ex.Message);
                // Return an error response
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            try
            {
                // Fetch the claim using the claim ID
                var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);

                if (claim == null)
                {
                    return NotFound("Claim not found.");
                }

                // Assuming you store CoordinatorId in the session after coordinator login
                var coordinatorId = HttpContext.Session.GetInt32("CoordinatorId");

                if (!coordinatorId.HasValue)
                {
                    return BadRequest("Coordinator ID not found in session.");
                }

                // Set the CoordinatorId and update the status to 'verified'
                claim.CoordinatorId = coordinatorId.Value;
                claim.Status = "verified"; // Update claim status to 'verified'

                // Fetch the coordinator to update the VerificationDate
                var coordinator = _context.Coordinators.FirstOrDefault(c => c.CoordinatorId == coordinatorId.Value);

                if (coordinator == null)
                {
                    return NotFound("Coordinator not found.");
                }

                // Set the current date and time as VerificationDate
                coordinator.VerificationDate = DateTime.Now;

                // Save the changes to both the claim and coordinator
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                Console.WriteLine(ex.Message);
                // Return an error response
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public IActionResult RejectClaim(int id)
        {
            try
            {
                var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == id);

                if (claim == null)
                {
                    return NotFound("Claim not found.");
                }

                // Assuming you store CoordinatorId in the session after coordinator login
                var coordinatorId = HttpContext.Session.GetInt32("CoordinatorId");

                if (!coordinatorId.HasValue)
                {
                    return BadRequest("Coordinator ID not found in session.");
                }

                // Set the status to 'rejected' and update the CoordinatorId
                claim.CoordinatorId = coordinatorId.Value;
                claim.Status = "rejected";

                // Fetch the coordinator to update the VerificationDate
                var coordinator = _context.Coordinators.FirstOrDefault(c => c.CoordinatorId == coordinatorId.Value);

                if (coordinator == null)
                {
                    return NotFound("Coordinator not found.");
                }

                // Set the current date and time as VerificationDate
                coordinator.VerificationDate = DateTime.Now;

                // Save the changes to both the claim and coordinator
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                Console.WriteLine(ex.Message);
                // Return an error response
                return StatusCode(500, "Internal server error.");
            }
        }

        private (bool isHourlyRateValid, bool isHoursWorkedValid, bool isOvertimeValid) IsClaimValid(LecturerClaimModel claim)
        {
            // Hourly Rate validation (50 - 400) This is just the parameters that I defined, you can adjust them as needed
            bool isHourlyRateValid = claim.HourlyRate >= 50 && claim.HourlyRate <= 400;

            // Hours Worked validation (35 - 80)
            bool isHoursWorkedValid = claim.HoursWorked >= 35 && claim.HoursWorked <= 80;

            // Overtime validation (cannot exceed 10 hours)
            bool isOvertimeValid = claim.OvertimeWorked <= 10;

            // Return tuple with all validation results
            return (isHourlyRateValid, isHoursWorkedValid, isOvertimeValid);
        }
    }
}
