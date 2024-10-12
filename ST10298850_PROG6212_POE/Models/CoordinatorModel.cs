﻿using System.ComponentModel.DataAnnotations;

namespace ST10298850_PROG6212_POE.Models
{
    public class CoordinatorModel
    {
        [Key]
        public int CoordinatorId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Department { get; set; }
    }
}