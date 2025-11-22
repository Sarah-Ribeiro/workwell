using Microsoft.EntityFrameworkCore;
using WorkWell.Api.Database;
using WorkWell.Api.Models;
using WorkWell.Api.DTOs;

namespace WorkWell.Api.Services
{
    public interface IHealthDataService
    {
        Task<HealthDataResponseDto> CreateHealthDataAsync(HealthDataCreateDto dto);
        Task<List<HealthDataResponseDto>> GetUserHealthDataAsync(int userId, int limit = 50);
        Task<HealthDataResponseDto?> GetLatestHealthDataAsync(int userId);
        Task<Dictionary<string, object>> GetUserStatisticsAsync(int userId, int days = 7);
    }

    public class HealthDataService : IHealthDataService
    {
        private readonly WorkWellContext _context;

        public HealthDataService(WorkWellContext context)
        {
            _context = context;
        }

        public async Task<HealthDataResponseDto> CreateHealthDataAsync(HealthDataCreateDto dto)
        {
            var healthData = new HealthData
            {
                UserId = dto.UserId,
                HeartRate = dto.HeartRate,
                StressLevel = dto.StressLevel,
                NoiseLevel = dto.NoiseLevel,
                Temperature = dto.Temperature,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.HealthData.Add(healthData);
            await _context.SaveChangesAsync();

            return MapToResponseDto(healthData);
        }

        public async Task<List<HealthDataResponseDto>> GetUserHealthDataAsync(int userId, int limit = 50)
        {
            var data = await _context.HealthData
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .Take(limit)
                .ToListAsync();

            return data.Select(MapToResponseDto).ToList();
        }

        public async Task<HealthDataResponseDto?> GetLatestHealthDataAsync(int userId)
        {
            var data = await _context.HealthData
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefaultAsync();

            return data != null ? MapToResponseDto(data) : null;
        }

        public async Task<Dictionary<string, object>> GetUserStatisticsAsync(int userId, int days = 7)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);

            var data = await _context.HealthData
                .Where(h => h.UserId == userId && h.CreatedAt >= startDate)
                .ToListAsync();

            if (!data.Any())
            {
                return new Dictionary<string, object>
                {
                    { "message", "Nenhum dado encontrado" }
                };
            }

            return new Dictionary<string, object>
            {
                { "period_days", days },
                { "total_records", data.Count },
                { "avg_heart_rate", Math.Round(data.Average(d => d.HeartRate), 1) },
                { "max_heart_rate", data.Max(d => d.HeartRate) },
                { "min_heart_rate", data.Min(d => d.HeartRate) },
                { "avg_stress_level", Math.Round(data.Average(d => d.StressLevel), 2) },
                { "max_stress_level", Math.Round(data.Max(d => d.StressLevel), 2) },
                { "high_stress_count", data.Count(d => d.StressLevel >= 0.6) },
                { "high_stress_percentage", Math.Round((data.Count(d => d.StressLevel >= 0.6) * 100.0 / data.Count), 1) }
            };
        }

        private HealthDataResponseDto MapToResponseDto(HealthData data)
        {
            return new HealthDataResponseDto
            {
                Id = data.Id,
                UserId = data.UserId,
                HeartRate = data.HeartRate,
                StressLevel = data.StressLevel,
                StressCategory = data.StressCategory,
                HeartRateCategory = data.HeartRateCategory,
                NoiseLevel = data.NoiseLevel,
                Temperature = data.Temperature,
                CreatedAt = data.CreatedAt,
                Notes = data.Notes
            };
        }
    }
}