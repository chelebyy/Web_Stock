using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Common.Models;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// Rapor işlemleri için servis
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ReportService> _logger;
        private readonly ICacheService _cacheService;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _reportStoragePath;

        public ReportService(
            ApplicationDbContext dbContext,
            ILogger<ReportService> logger,
            ICacheService cacheService,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _cacheService = cacheService;
            _currentUserService = currentUserService;
            _reportStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            
            // Rapor klasörünü oluştur
            if (!Directory.Exists(_reportStoragePath))
            {
                Directory.CreateDirectory(_reportStoragePath);
            }
        }

        /// <inheritdoc />
        public async Task<string> QueueReportAsync(string reportType, Dictionary<string, string> parameters, string userId)
        {
            _logger.LogInformation("Yeni rapor isteği ekleniyor. Tip: {ReportType}, Kullanıcı: {UserId}", reportType, userId);
            
            var report = new Report
            {
                Id = Guid.NewGuid().ToString(),
                ReportType = reportType,
                Parameters = System.Text.Json.JsonSerializer.Serialize(parameters),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = (int)ReportStatus.Pending
            };
            
            _dbContext.Reports.Add(report);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            
            _logger.LogInformation("Rapor isteği eklendi. ID: {ReportId}", report.Id);
            
            return report.Id;
        }

        /// <inheritdoc />
        public async Task ProcessPendingReportsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Bekleyen raporlar işleniyor...");
            
            var pendingReports = await _dbContext.Reports
                .Where(r => r.Status == (int)ReportStatus.Pending)
                .OrderBy(r => r.CreatedAt)
                .Take(10) // Bir seferde en fazla 10 rapor işle
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            
            _logger.LogInformation("{Count} adet bekleyen rapor bulundu.", pendingReports.Count);
            
            foreach (var report in pendingReports)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("İşlem iptal edildi.");
                    break;
                }
                
                await ProcessReportAsync(report, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<ReportStatus> GetReportStatusAsync(string reportId)
        {
            var cacheKey = $"report_status_{reportId}";
            
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                var report = await _dbContext.Reports
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == reportId)
                    .ConfigureAwait(false);
                
                return report != null ? (ReportStatus)report.Status : ReportStatus.Failed;
            }, 5).ConfigureAwait(false); // 5 dakika önbellekte tut
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ReportInfo>> GetUserReportsAsync(string userId)
        {
            var cacheKey = $"user_reports_{userId}";
            
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                var reports = await _dbContext.Reports
                    .AsNoTracking()
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync()
                    .ConfigureAwait(false);
                
                return reports.Select(r => new ReportInfo
                {
                    Id = r.Id,
                    ReportType = r.ReportType,
                    Parameters = r.Parameters != null 
                        ? System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(r.Parameters) 
                        : new Dictionary<string, string>(),
                    UserId = r.UserId,
                    CreatedAt = r.CreatedAt,
                    CompletedAt = r.CompletedAt,
                    Status = (ReportStatus)r.Status,
                    ErrorMessage = r.ErrorMessage,
                    FileName = r.FileName
                }).ToList();
            }, 10).ConfigureAwait(false); // 10 dakika önbellekte tut
        }

        /// <inheritdoc />
        public async Task<ReportFile> DownloadReportAsync(string reportId)
        {
            var report = await _dbContext.Reports
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reportId)
                .ConfigureAwait(false);
            
            if (report == null || report.Status != (int)ReportStatus.Completed || string.IsNullOrEmpty(report.FileName))
            {
                _logger.LogWarning("Rapor bulunamadı veya tamamlanmadı. ID: {ReportId}", reportId);
                return null;
            }
            
            var filePath = Path.Combine(_reportStoragePath, report.FileName);
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Rapor dosyası bulunamadı. Dosya: {FilePath}", filePath);
                return null;
            }
            
            var content = await File.ReadAllBytesAsync(filePath).ConfigureAwait(false);
            var contentType = GetContentType(report.FileName);
            
            return new ReportFile
            {
                FileName = report.FileName,
                Content = content,
                ContentType = contentType
            };
        }
        
        private async Task ProcessReportAsync(Report report, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rapor işleniyor. ID: {ReportId}, Tip: {ReportType}", report.Id, report.ReportType);
            
            try
            {
                // Raporu işleniyor olarak işaretle
                report.Status = (int)ReportStatus.Processing;
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                // Önbellekteki rapor durumunu temizle
                await _cacheService.RemoveAsync($"report_status_{report.Id}").ConfigureAwait(false);
                
                // Rapor tipine göre işlem yap
                switch (report.ReportType)
                {
                    case "UserActivity":
                        await GenerateUserActivityReportAsync(report, cancellationToken).ConfigureAwait(false);
                        break;
                    case "StockMovement":
                        await GenerateStockMovementReportAsync(report, cancellationToken).ConfigureAwait(false);
                        break;
                    default:
                        throw new NotSupportedException($"Desteklenmeyen rapor tipi: {report.ReportType}");
                }
                
                // Raporu tamamlandı olarak işaretle
                report.Status = (int)ReportStatus.Completed;
                report.CompletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                _logger.LogInformation("Rapor başarıyla oluşturuldu. ID: {ReportId}", report.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor oluşturma sırasında hata oluştu. ID: {ReportId}", report.Id);
                
                // Raporu hata olarak işaretle
                report.Status = (int)ReportStatus.Failed;
                report.ErrorMessage = ex.Message;
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            
            // Önbellekteki kullanıcı raporlarını temizle
            await _cacheService.RemoveAsync($"user_reports_{report.UserId}").ConfigureAwait(false);
        }
        
        private async Task GenerateUserActivityReportAsync(Report report, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kullanıcı aktivite raporu oluşturuluyor. ID: {ReportId}", report.Id);
            
            // Parametreleri al
            var parameters = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(report.Parameters);
            var startDateStr = parameters != null && parameters.ContainsKey("startDate") ? parameters["startDate"] : null;
            var endDateStr = parameters != null && parameters.ContainsKey("endDate") ? parameters["endDate"] : null;
            var userIdStr = parameters != null && parameters.ContainsKey("userId") ? parameters["userId"] : null;
            
            // Parametreleri doğrula ve dönüştür
            if (!DateTime.TryParse(startDateStr, out var startDate))
            {
                startDate = DateTime.UtcNow.AddDays(-30);
            }
            
            if (!DateTime.TryParse(endDateStr, out var endDate))
            {
                endDate = DateTime.UtcNow;
            }
            
            int? userId = null;
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out var parsedUserId))
            {
                userId = parsedUserId;
            }
            
            // Aktivite kayıtlarını sorgula
            var query = _dbContext.ActivityLogs.AsQueryable();
            
            if (userId.HasValue)
            {
                query = query.Where(a => a.UserId == userId);
            }
            
            query = query.Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate);
            
            var activities = await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            
            // Rapor dosyasını oluştur
            var fileName = $"UserActivity_{report.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            var filePath = Path.Combine(_reportStoragePath, fileName);
            
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync("ID,Kullanıcı ID,İşlem,Detay,Tarih").ConfigureAwait(false);
                
                foreach (var activity in activities)
                {
                    await writer.WriteLineAsync(
                        $"{activity.Id},{activity.UserId},{activity.ActivityType},{activity.Description},{activity.Timestamp:yyyy-MM-dd HH:mm:ss}"
                    ).ConfigureAwait(false);
                }
            }
            
            // Raporu güncelle
            report.Status = (int)ReportStatus.Completed;
            report.FileName = fileName;
            report.CompletedAt = DateTime.UtcNow;
            
            _dbContext.Reports.Update(report);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        
        private async Task GenerateStockMovementReportAsync(Report report, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stok hareket raporu oluşturuluyor. ID: {ReportId}", report.Id);
            
            var parameters = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(report.Parameters);
            
            // Parametreleri al
            var startDateStr = parameters.GetValueOrDefault("startDate");
            var endDateStr = parameters.GetValueOrDefault("endDate");
            var productId = parameters.GetValueOrDefault("productId");
            
            DateTime startDate = string.IsNullOrEmpty(startDateStr) ? DateTime.UtcNow.AddDays(-30) : DateTime.Parse(startDateStr);
            DateTime endDate = string.IsNullOrEmpty(endDateStr) ? DateTime.UtcNow : DateTime.Parse(endDateStr);
            
            // Rapor verilerini al
            var query = _dbContext.StockMovements.AsNoTracking();
            
            if (!string.IsNullOrEmpty(productId) && int.TryParse(productId, out int productIdInt))
            {
                query = query.Where(s => s.ProductId == productIdInt);
            }
            
            query = query.Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate);
            
            var movements = await query
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            
            // Rapor dosyasını oluştur
            var fileName = $"StockMovement_{report.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            var filePath = Path.Combine(_reportStoragePath, fileName);
            
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync("ID,Ürün ID,Miktar,Hareket Tipi,Açıklama,Oluşturan Kullanıcı,Tarih").ConfigureAwait(false);
                
                foreach (var movement in movements)
                {
                    await writer.WriteLineAsync(
                        $"{movement.Id},{movement.ProductId},{movement.Quantity},{movement.MovementType},{movement.Description},{movement.CreatedBy},{movement.CreatedAt:yyyy-MM-dd HH:mm:ss}"
                    ).ConfigureAwait(false);
                }
            }
            
            report.FileName = fileName;
        }
        
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            
            return extension switch
            {
                ".csv" => "text/csv",
                ".pdf" => "application/pdf",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
    }
} 