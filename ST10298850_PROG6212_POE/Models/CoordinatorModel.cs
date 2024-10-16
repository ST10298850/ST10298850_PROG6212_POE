using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Models
{
    public class CoordinatorModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoordinatorId { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }

        public string? Department { get; set; }
    }
}
