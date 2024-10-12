using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ST10298850_PROG6212_POE.Models
{
    public class LecturerModel
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Department { get; set; }

        public ICollection<Claim> Claims { get; set; }
    }
}
