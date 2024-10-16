using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Models
{
    public class LecturerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LecturerId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Initialize with default value

        [Required]
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty; // Initialize with default value

        public ICollection<LecturerClaimModel> Claims { get; set; } = new List<LecturerClaimModel>(); // Initialize with default value
    }
}
