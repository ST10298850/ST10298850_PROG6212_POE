using System.ComponentModel.DataAnnotations;

namespace ST10298850_PROG6212_POE.Models
{
    public class HRModel
    {
        [Key]
        public int hrId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
