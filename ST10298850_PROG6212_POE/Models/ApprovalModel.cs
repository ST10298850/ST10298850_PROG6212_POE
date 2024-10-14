using System.ComponentModel.DataAnnotations;
using ST10298850_PROG6212_POE.Models;  // This is correct

namespace ST10298850_PROG6212_POE.Models
{
    public class ApprovalModel
    {
        [Key]
        public int ApprovalId { get; set; }

        [Required]
        public int ClaimId { get; set; }
        public LecturerClaimModel Claim { get; set; }

        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}
