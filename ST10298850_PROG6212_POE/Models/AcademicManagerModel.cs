using System.ComponentModel.DataAnnotations;
using ST10298850_PROG6212_POE.Models;  // This is correct
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10298850_PROG6212_POE.Models
{
    public class AcademicManagerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? Department { get; set; }
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
