using System;
using System.ComponentModel.DataAnnotations;

namespace WorkWell.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Range(18, 100)]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; }

        [Range(40, 100)]
        public int BaselineHeartRate { get; set; } = 70;

        public bool IsActive { get; set; } = true;
    }
}