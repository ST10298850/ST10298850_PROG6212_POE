using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ILogger<ClaimController> _logger;
        private readonly AppDbContext _context;

        public ClaimController(ILogger<ClaimController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult ClaimPageView()
        {
            int? lecturerId = HttpContext.Session.GetInt32("LecturerId");

            if (!lecturerId.HasValue)
            {
                TempData["ErrorMessage"] = "Session expired or Lecturer not logged in.";
                return RedirectToAction("LoginPage"); // or any other action for logging in
            }

            ViewBag.LecturerId = lecturerId.Value; // Pass lecturerId to the view
            return View();
        }

        [HttpGet]
        public IActionResult GetClaims()
        {
            int? lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (!lecturerId.HasValue)
            {
                return Unauthorized();
            }

            var claims = _context.Claims
                .Where(c => c.LecturerId == lecturerId.Value)
                .Select(c => new
                {
                    c.ClaimId,
                    c.SubmissionDate, // Retrieve SubmissionDate from claims table
                    c.Status // Retrieve Status from LecturerClaimModel
                })
                .ToList();

            return Json(claims);
        }


        [HttpPost]
        public IActionResult SubmitClaim(int lecturerId, decimal hoursWorked, decimal overtimeWorked, decimal hourlyRate, string documentName, IFormFile documentFile)
        {
            _logger.LogInformation("SubmitClaim method started.");

            var lecturer = _context.Lecturers?.FirstOrDefault(l => l.LecturerId == lecturerId);
            if (lecturer == null)
            {
                _logger.LogWarning("Lecturer not found: {LecturerId}", lecturerId);
                TempData["ErrorMessage"] = "Lecturer not found.";
                return RedirectToAction("ClaimPageView");
            }

            var newClaim = new LecturerClaimModel
            {
                LecturerId = lecturerId,
                HoursWorked = hoursWorked,
                OvertimeWorked = overtimeWorked,
                HourlyRate = hourlyRate, // Add hourly rate to the claim model
                SubmissionDate = DateTime.Now,
                Documents = new List<DocumentModel>(),
            };

            if (documentFile != null && documentFile.Length > 0)
            {
                // Validate file type
                var allowedFileTypes = new List<string>
    {
        "application/pdf",         // PDF
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // Word (DOCX)
        "application/msword",      // Word (DOC)
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // Excel (XLSX)
        "application/vnd.ms-excel" // Excel (XLS)
    };

                if (!allowedFileTypes.Contains(documentFile.ContentType))
                {
                    TempData["ErrorMessage"] = "Invalid file type. Please upload a PDF, Word, or Excel document.";
                    return RedirectToAction("ClaimPageView");
                }

                // Validate file size (e.g., max 2MB)
                if (documentFile.Length > 2 * 1024 * 1024) // 2MB in bytes
                {
                    TempData["ErrorMessage"] = "File size exceeds the limit of 2MB.";
                    return RedirectToAction("ClaimPageView");
                }

                using (var memoryStream = new MemoryStream())
                {
                    documentFile.CopyTo(memoryStream);
                    var fileData = memoryStream.ToArray();

                    var newDocument = new DocumentModel
                    {
                        DocumentName = documentName, // Keep the file name provided by the user
                        Claim = newClaim,
                        FileData = fileData,
                        FileType = documentFile.ContentType
                    };

                    newClaim.Documents.Add(newDocument);
                }
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data.";
                return RedirectToAction("ClaimPageView");
            }

            try
            {
                _context.Claims?.Add(newClaim);
                _context.SaveChanges();
                _logger.LogInformation("Claim saved successfully.");
                TempData["SuccessMessage"] = "Claim submitted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving claim.");
                TempData["ErrorMessage"] = "Error saving claim.";
            }

            return RedirectToAction("ClaimPageView");
        }
    }
}
