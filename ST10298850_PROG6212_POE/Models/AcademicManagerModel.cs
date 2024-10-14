using System.ComponentModel.DataAnnotations;
using ST10298850_PROG6212_POE.Models;  // This is correct

namespace ST10298850_PROG6212_POE.Models
{
    public class AcademicManagerModel
    {
        [Key]
        public int ManagerId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Department { get; set; }
    }
}
