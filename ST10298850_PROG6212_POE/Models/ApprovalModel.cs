using System.ComponentModel.DataAnnotations;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Models
{
    public class ApprovalModel
    {
        [Key]
        public int ApprovalId { get; set; }

        [Required]
        public int ClaimId { get; set; }
        public LecturerClaimModel Claim { get; set; } = null!; // Use null-forgiving operator

        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty; // Initialize with default value

        public string Comments { get; set; } = string.Empty; // Initialize with default value
    }
}
