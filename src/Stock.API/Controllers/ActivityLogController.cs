using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class ActivityLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ActivityLogController> _logger;
        private readonly IHostEnvironment _environment;

        public ActivityLogController(
            ApplicationDbContext context,
            ILogger<ActivityLogController> logger,
            IHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
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
        public async Task<IActionResult> CreateActivityLog([FromBody] object logData)
        {
            try
            {
                _logger.LogInformation($"Tekil aktivite log istegi alındı");
                
                // Frontend'den gelen log içeriğini detaylı logla
                var logJson = JsonSerializer.Serialize(logData);
                _logger.LogInformation($"Gelen veri: {logJson}");
                
                // Frontend modelini backend modeline dönüştür
                var log = new ActivityLog();
                
                try
                {
                    // Dynamic olarak çözümle
                    var dynamicLog = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(logJson);
                    
                    // Zorunlu alanları doldur
                    if (dynamicLog.TryGetValue("userId", out var userIdElement) && userIdElement.ValueKind != JsonValueKind.Null)
                    {
                        try
                        {
                            int userId = userIdElement.GetInt32();
                            
                            // Kullanıcı ID'sinin veritabanında olup olmadığını kontrol et
                            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                            
                            if (userExists)
                            {
                                log.UserId = userId;
                            }
                            else
                            {
                                _logger.LogWarning($"Belirtilen userId ({userId}) veritabanında bulunamadı. UserId null olarak ayarlanıyor.");
                                log.UserId = null; // Kullanıcı bulunamadıysa null olarak ayarla
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"userId alanı int'e dönüştürülemedi: {ex.Message}. UserId null olarak ayarlanıyor.");
                            log.UserId = null; // Hata durumunda null olarak ayarla
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Log kaydında userId bulunamadı veya null. UserId null olarak ayarlanıyor.");
                        log.UserId = null; // UserId yoksa null olarak ayarla
                    }
                    
                    if (dynamicLog.TryGetValue("username", out var usernameElement) && usernameElement.ValueKind != JsonValueKind.Null)
                    {
                        log.Username = usernameElement.GetString() ?? "admin";
                    }
                    else
                    {
                        log.Username = "admin";
                    }
                    
                    if (dynamicLog.TryGetValue("activityType", out var activityTypeElement) && activityTypeElement.ValueKind != JsonValueKind.Null)
                    {
                        log.ActivityType = activityTypeElement.GetString() ?? "unknown";
                    }
                    else
                    {
                        log.ActivityType = "unknown";
                    }
                    
                    if (dynamicLog.TryGetValue("description", out var descriptionElement) && descriptionElement.ValueKind != JsonValueKind.Null)
                    {
                        log.Description = descriptionElement.GetString() ?? "No description";
                    }
                    else
                    {
                        log.Description = "No description";
                    }
                    
                    if (dynamicLog.TryGetValue("timestamp", out var timestampElement) && timestampElement.ValueKind != JsonValueKind.Null)
                    {
                        try
                        {
                            log.Timestamp = timestampElement.GetDateTime();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"timestamp alanı DateTime'a dönüştürülemedi: {ex.Message}. Şu anki zaman kullanılacak.");
                            log.Timestamp = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        log.Timestamp = DateTime.UtcNow;
                    }
                    
                    // İsteğe bağlı alanlar
                    var additionalInfo = new Dictionary<string, object>();
                    
                    if (dynamicLog.TryGetValue("status", out var statusElement) && statusElement.ValueKind != JsonValueKind.Null)
                    {
                        additionalInfo["status"] = statusElement.GetString() ?? "info";
                    }
                    
                    if (dynamicLog.TryGetValue("details", out var detailsElement) && detailsElement.ValueKind != JsonValueKind.Null)
                    {
                        additionalInfo["details"] = detailsElement.ToString();
                    }
                    
                    if (dynamicLog.TryGetValue("ipAddress", out var ipAddressElement) && ipAddressElement.ValueKind != JsonValueKind.Null)
                    {
                        additionalInfo["ipAddress"] = ipAddressElement.GetString() ?? string.Empty;
                    }
                    
                    // AdditionalInfo alanını set et
                    log.AdditionalInfo = JsonSerializer.Serialize(additionalInfo);
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError($"JSON dönüşüm hatası: {jsonEx.Message}");
                    return BadRequest($"JSON dönüşüm hatası: {jsonEx.Message}");
                }
                
                _logger.LogInformation($"Veritabanına log kaydediliyor. UserId: {log.UserId}, ActivityType: {log.ActivityType}");
                _context.ActivityLogs.Add(log);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Log başarıyla kaydedildi", log });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Aktivite logu kaydedilirken hata oluştu: {ex.Message}");
                _logger.LogError($"İç hata: {ex.InnerException?.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                
                if (_environment.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        error = "Aktivite logu kaydedilirken bir hata oluştu.",
                        message = ex.Message,
                        innerException = ex.InnerException?.Message,
                        stackTrace = ex.StackTrace
                    });
                }
                
                return StatusCode(500, "Aktivite logu kaydedilirken bir hata oluştu.");
            }
        }

        [HttpPost("bulk-activity")]
        public async Task<IActionResult> CreateBulkActivityLogs([FromBody] List<object> logsData)
        {
            try
            {
                _logger.LogInformation($"Toplu aktivite log istegi alındı. Eleman sayısı: {logsData.Count}");
                
                // Frontend'den gelen log içeriğini detaylı logla
                _logger.LogInformation($"Gelen veri: {JsonSerializer.Serialize(logsData)}");
                
                List<ActivityLog> logs = new List<ActivityLog>();
                
                foreach (var logData in logsData)
                {
                    try
                    {
                        // Frontend'den gelen modeli string olarak al ve logla
                        var logJson = JsonSerializer.Serialize(logData);
                        _logger.LogInformation($"İşlenen log: {logJson}");
                        
                        // Frontend modelini backend modeline dönüştür
                        var log = new ActivityLog();
                        
                        try
                        {
                            // Dynamic olarak çözümle
                            var dynamicLog = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(logJson);
                            
                            // Zorunlu alanları doldur
                            if (dynamicLog.TryGetValue("userId", out var userIdElement) && userIdElement.ValueKind != JsonValueKind.Null)
                            {
                                try
                                {
                                    int userId = userIdElement.GetInt32();
                                    
                                    // Kullanıcı ID'sinin veritabanında olup olmadığını kontrol et
                                    var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                                    
                                    if (userExists)
                                    {
                                        log.UserId = userId;
                                    }
                                    else
                                    {
                                        _logger.LogWarning($"Belirtilen userId ({userId}) veritabanında bulunamadı. UserId null olarak ayarlanıyor.");
                                        log.UserId = null; // Kullanıcı bulunamadıysa null olarak ayarla
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning($"userId alanı int'e dönüştürülemedi: {ex.Message}. UserId null olarak ayarlanıyor.");
                                    log.UserId = null; // Hata durumunda null olarak ayarla
                                }
                            }
                            else
                            {
                                _logger.LogWarning("Log kaydında userId bulunamadı veya null. UserId null olarak ayarlanıyor.");
                                log.UserId = null; // UserId yoksa null olarak ayarla
                            }
                            
                            if (dynamicLog.TryGetValue("username", out var usernameElement) && usernameElement.ValueKind != JsonValueKind.Null)
                            {
                                log.Username = usernameElement.GetString() ?? "admin";
                            }
                            else
                            {
                                log.Username = "admin";
                            }
                            
                            if (dynamicLog.TryGetValue("activityType", out var activityTypeElement) && activityTypeElement.ValueKind != JsonValueKind.Null)
                            {
                                log.ActivityType = activityTypeElement.GetString() ?? "unknown";
                            }
                            else
                            {
                                log.ActivityType = "unknown";
                            }
                            
                            if (dynamicLog.TryGetValue("description", out var descriptionElement) && descriptionElement.ValueKind != JsonValueKind.Null)
                            {
                                log.Description = descriptionElement.GetString() ?? "No description";
                            }
                            else
                            {
                                log.Description = "No description";
                            }
                            
                            if (dynamicLog.TryGetValue("timestamp", out var timestampElement) && timestampElement.ValueKind != JsonValueKind.Null)
                            {
                                try
                                {
                                    log.Timestamp = timestampElement.GetDateTime();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning($"timestamp alanı DateTime'a dönüştürülemedi: {ex.Message}. Şu anki zaman kullanılacak.");
                                    log.Timestamp = DateTime.UtcNow;
                                }
                            }
                            else
                            {
                                log.Timestamp = DateTime.UtcNow;
                            }
                            
                            // İsteğe bağlı alanlar
                            var additionalInfo = new Dictionary<string, object>();
                            
                            if (dynamicLog.TryGetValue("status", out var statusElement) && statusElement.ValueKind != JsonValueKind.Null)
                            {
                                additionalInfo["status"] = statusElement.GetString() ?? "info";
                            }
                            
                            if (dynamicLog.TryGetValue("details", out var detailsElement) && detailsElement.ValueKind != JsonValueKind.Null)
                            {
                                additionalInfo["details"] = detailsElement.ToString();
                            }
                            
                            if (dynamicLog.TryGetValue("ipAddress", out var ipAddressElement) && ipAddressElement.ValueKind != JsonValueKind.Null)
                            {
                                additionalInfo["ipAddress"] = ipAddressElement.GetString() ?? string.Empty;
                            }
                            
                            // AdditionalInfo alanını set et
                            log.AdditionalInfo = JsonSerializer.Serialize(additionalInfo);
                        }
                        catch (JsonException jsonEx)
                        {
                            _logger.LogError($"JSON dönüşüm hatası: {jsonEx.Message}. Log atlanacak.");
                            continue;
                        }
                        
                        logs.Add(log);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Bir log öğesi işlenirken hata oluştu: {ex.Message}, {ex.StackTrace}");
                        // Bir log'un dönüşümünde hata olsa bile diğerlerini işlemeye devam et
                    }
                }
                
                if (logs.Count > 0)
                {
                    _logger.LogInformation($"Veritabanına {logs.Count} adet log kaydediliyor");
                    _context.ActivityLogs.AddRange(logs);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = $"{logs.Count} adet log başarıyla kaydedildi", logs });
                }
                else
                {
                    _logger.LogWarning("İşlenebilecek log bulunamadı");
                    return BadRequest("İşlenebilecek log bulunamadı. Gönderilen veriler geçersiz format içeriyor olabilir.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Toplu aktivite logları kaydedilirken hata oluştu: {ex.Message}");
                _logger.LogError($"İç hata: {ex.InnerException?.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                
                if (_environment.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        error = "Toplu aktivite logları kaydedilirken bir hata oluştu.",
                        message = ex.Message,
                        innerException = ex.InnerException?.Message,
                        stackTrace = ex.StackTrace
                    });
                }
                
                return StatusCode(500, "Toplu aktivite logları kaydedilirken bir hata oluştu.");
            }
        }
    }
} 