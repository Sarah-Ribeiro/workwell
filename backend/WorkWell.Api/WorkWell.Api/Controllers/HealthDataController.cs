using Microsoft.AspNetCore.Mvc;
using WorkWell.Api.DTOs;
using WorkWell.Api.Services;

namespace WorkWell.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthDataController : ControllerBase
    {
        private readonly IHealthDataService _healthDataService;
        private readonly ILogger<HealthDataController> _logger;

        public HealthDataController(
            IHealthDataService healthDataService,
            ILogger<HealthDataController> logger)
        {
            _healthDataService = healthDataService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<HealthDataResponseDto>> CreateHealthData(
            [FromBody] HealthDataCreateDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Recebendo dados - UserId: {UserId}, HR: {HeartRate}",
                    dto.UserId, dto.HeartRate);

                var result = await _healthDataService.CreateHealthDataAsync(dto);

                return CreatedAtAction(
                    nameof(GetLatestHealthData),
                    new { userId = result.UserId },
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar registro");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<HealthDataResponseDto>>> GetUserHealthData(
            int userId,
            [FromQuery] int limit = 50)
        {
            try
            {
                var data = await _healthDataService.GetUserHealthDataAsync(userId, limit);

                if (!data.Any())
                {
                    return NotFound(new { message = "Nenhum dado encontrado" });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}/latest")]
        public async Task<ActionResult<HealthDataResponseDto>> GetLatestHealthData(int userId)
        {
            try
            {
                var data = await _healthDataService.GetLatestHealthDataAsync(userId);

                if (data == null)
                {
                    return NotFound(new { message = "Nenhum dado encontrado" });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar último registro");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}/statistics")]
        public async Task<ActionResult<Dictionary<string, object>>> GetUserStatistics(
            int userId,
            [FromQuery] int days = 7)
        {
            try
            {
                var stats = await _healthDataService.GetUserStatisticsAsync(userId, days);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular estatísticas");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }
    }
}