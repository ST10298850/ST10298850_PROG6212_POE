using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Models
{
    public class LecturerClaimModel
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public int LecturerId { get; set; }
        public LecturerModel? Lecturer { get; set; }

        public DateTime SubmissionDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]  // Set precision and scale for HoursWorked
        public decimal HoursWorked { get; set; }

        [Column(TypeName = "decimal(18, 2)")]  // Set precision and scale for OvertimeWorked
        public decimal OvertimeWorked { get; set; }
        // New Status property
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18, 2)")]  // Set precision and scale for HourlyRate
        public decimal HourlyRate { get; set; }

        public ICollection<DocumentModel> Documents { get; set; } = new List<DocumentModel>(); // Initialize with default value
        public ApprovalModel Approval { get; set; } = null!; // Use null-forgiving operator
    }
}
