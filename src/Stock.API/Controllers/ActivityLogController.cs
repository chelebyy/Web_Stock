using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class ActivityLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ActivityLogController> _logger;

        public ActivityLogController(
            ApplicationDbContext context,
            ILogger<ActivityLogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("activity")]
        public async Task<IActionResult> GetActivityLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.ActivityLogs
                    .Include(l => l.User)
                    .OrderByDescending(l => l.Timestamp)
                    .AsQueryable();

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var logs = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    Logs = logs,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = page
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Aktivite logları alınırken hata oluştu: {ex.Message}");
                return StatusCode(500, "Aktivite logları alınırken bir hata oluştu.");
            }
        }

        [HttpPost("activity")]
        public async Task<IActionResult> CreateActivityLog([FromBody] ActivityLog log)
        {
            try
            {
                _context.ActivityLogs.Add(log);
                await _context.SaveChangesAsync();

                return Ok(log);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Aktivite logu kaydedilirken hata oluştu: {ex.Message}");
                return StatusCode(500, "Aktivite logu kaydedilirken bir hata oluştu.");
            }
        }

        [HttpPost("bulk-activity")]
        public async Task<IActionResult> CreateBulkActivityLogs([FromBody] List<ActivityLog> logs)
        {
            try
            {
                _context.ActivityLogs.AddRange(logs);
                await _context.SaveChangesAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Toplu aktivite logları kaydedilirken hata oluştu: {ex.Message}");
                return StatusCode(500, "Toplu aktivite logları kaydedilirken bir hata oluştu.");
            }
        }
    }
} 