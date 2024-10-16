using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST10298850_PROG6212_POE.Data;
using ST10298850_PROG6212_POE.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            if (lecturerId.HasValue)
            {
                ViewBag.LecturerId = lecturerId.Value;

                // Fetch claims for the signed-in lecturer
                var claims = _context.Claims
                    .Where(c => c.LecturerId == lecturerId.Value)
                    .Select(c => new
                    {
                        c.ClaimId,
                        c.SubmissionDate,
                        c.Approval.ApprovalStatus,
                        c.Approval.Comments
                    })
                    .ToList();

                ViewBag.Claims = claims;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult SubmitClaim(int lecturerId, decimal hoursWorked, decimal overtimeWorked, string documentName, IFormFile documentFile)
        {
            _logger.LogInformation("SubmitClaim method started.");

            var lecturer = _context.Lecturers?.FirstOrDefault(l => l.LecturerId == lecturerId);
            if (lecturer == null)
            {
                _logger.LogWarning("Lecturer not found: {LecturerId}", lecturerId);
                return NotFound("Lecturer not found");
            }

            var newClaim = new LecturerClaimModel
            {
                LecturerId = lecturerId,
                HoursWorked = hoursWorked,
                OvertimeWorked = overtimeWorked,
                SubmissionDate = DateTime.Now,
                Documents = new List<DocumentModel>(),
            };

            if (documentFile != null && documentFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    documentFile.CopyTo(memoryStream);
                    var fileData = memoryStream.ToArray();

                    var newDocument = new DocumentModel
                    {
                        DocumentName = documentName,
                        Claim = newClaim,
                        FileData = fileData,
                        FileType = documentFile.ContentType
                    };

                    newClaim.Documents.Add(newDocument);
                }
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return View("ClaimPageView");
            }

            try
            {
                _context.Claims?.Add(newClaim);
                _context.SaveChanges();
                _logger.LogInformation("Claim saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving claim.");
                return StatusCode(500, "Internal server error");
            }

            return RedirectToAction("ClaimPageView");
        }
    }
}
