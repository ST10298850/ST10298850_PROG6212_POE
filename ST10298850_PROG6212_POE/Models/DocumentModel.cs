using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Models
{
    public class DocumentModel
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public string DocumentName { get; set; } = string.Empty; // Name of the document (e.g., "report.pdf")

        public int ClaimId { get; set; }
        public LecturerClaimModel Claim { get; set; } = null!; // Reference to the associated claim

        // Store the actual document data as a binary field
        [Required]
        [Column(TypeName = "varbinary(max)")] // SQL Server-specific annotation for large binary data
        public byte[] FileData { get; set; } = null!; // Binary data of the document

        [Required]
        public string FileType { get; set; } = string.Empty; // MIME type (e.g., "application/pdf")
    }
}
 