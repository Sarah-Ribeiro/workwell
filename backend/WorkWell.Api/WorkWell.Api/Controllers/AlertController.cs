using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWell.Api.Database;
using WorkWell.Api.Models;

namespace WorkWell.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly WorkWellContext _context;
        public AlertController(WorkWellContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts()
        {
            return await _context.Alerts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alert>> GetAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert == null) return NotFound();
            return alert;
        }

        [HttpPost]
        public async Task<ActionResult<Alert>> CreateAlert(Alert alert)
        {
            alert.CreatedAt = DateTime.Now;
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlert(int id, Alert alert)
        {
            if (id != alert.Id) return BadRequest();
            _context.Entry(alert).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Alerts.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert == null) return NotFound();
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
