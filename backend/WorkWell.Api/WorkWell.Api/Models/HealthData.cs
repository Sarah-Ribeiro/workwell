using System;
using System.ComponentModel.DataAnnotations;

namespace WorkWell.Api.Models
{
    public class HealthData
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        public DateTime CreatedAt { get; set; }

        public string? Notes { get; set; }

        public string StressCategory => StressLevel switch
        {
            < 0.3 => "Baixo",
            < 0.6 => "Moderado",
            < 0.8 => "Alto",
            _ => "Muito Alto"
        };

        public string HeartRateCategory => HeartRate switch
        {
            < 60 => "Bradicardia",
            <= 100 => "Normal",
            <= 120 => "Elevado",
            _ => "Taquicardia"
        };
    }
}