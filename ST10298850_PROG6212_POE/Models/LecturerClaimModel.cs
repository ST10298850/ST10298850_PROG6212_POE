using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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

        [Column(TypeName = "decimal(18, 2)")]
        public decimal HoursWorked { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OvertimeWorked { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18, 2)")]
        public decimal HourlyRate { get; set; }

        // New property for CoordinatorId
        public int? CoordinatorId { get; set; }  // Nullable if a claim can exist without a coordinator

        public ICollection<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
        public ApprovalModel Approval { get; set; } = null!;

        // New navigation property
        public CoordinatorModel? Coordinator { get; set; }
    }
}