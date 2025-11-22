using System;
using System.ComponentModel.DataAnnotations;

namespace WorkWell.Api.DTOs
{
    public class HealthDataCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(40, 200)]
        public int HeartRate { get; set; }

        [Required]
        [Range(0, 1)]
        public double StressLevel { get; set; }

        public double? NoiseLevel { get; set; }

        public double? Temperature { get; set; }

        public string? Notes { get; set; }
    }

    public class HealthDataResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HeartRate { get; set; }
        public double StressLevel { get; set; }
        public string StressCategory { get; set; } = string.Empty;
        public string HeartRateCategory { get; set; } = string.Empty;
        public double? NoiseLevel { get; set; }
        public double? Temperature { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
    }
}