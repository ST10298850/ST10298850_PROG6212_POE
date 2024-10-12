using System.ComponentModel.DataAnnotations;

namespace ST10298850_PROG6212_POE.Models
{
    public class ClaimModel
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public int LecturerId { get; set; }
        public LecturerModel Lecturer { get; set; }

        public DateTime SubmissionDate { get; set; }

        public decimal HoursWorked { get; set; }
        public decimal OvertimeWorked { get; set; }

        public ICollection<Document> Documents { get; set; }
        public Approval Approval { get; set; }
    }
}
