using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ST10298850_PROG6212_POE.Models
{
    public class DocumentModel
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public string DocumentName { get; set; }

        public int ClaimId { get; set; }
        public ClaimModel Claim { get; set; }
    }
}
