using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.ActivityLogs.Commands.CreateBulkActivityLogs
{
    public class CreateBulkActivityLogsCommandHandler : IRequestHandler<CreateBulkActivityLogsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBulkActivityLogsCommandHandler> _logger;

        public CreateBulkActivityLogsCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateBulkActivityLogsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(CreateBulkActivityLogsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Toplu aktivite log isteği işleniyor. Eleman sayısı: {request.LogsData.Count}");

            var logsToAdd = new List<ActivityLog>();
            foreach (var logData in request.LogsData)
            {
                try
                {
                    var log = await ParseLogData(logData, cancellationToken);
                    logsToAdd.Add(log);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Toplu log işlenirken bir hata oluştu. Log verisi: {logData.ToString()}");
                    // Optionally, continue processing other logs
                }
            }

            if (logsToAdd.Any())
            {
                await _unitOfWork.ActivityLogs.AddRangeAsync(logsToAdd, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"{logsToAdd.Count} adet log başarıyla veritabanına kaydedildi.");
            }
            else
            {
                _logger.LogWarning("Kaydedilecek geçerli log bulunamadı.");
            }
        }
        
        private async Task<ActivityLog> ParseLogData(JsonElement logData, CancellationToken cancellationToken)
        {
            var log = new ActivityLog();

            if (logData.TryGetProperty("userId", out var userIdElement) && userIdElement.ValueKind != JsonValueKind.Null && userIdElement.TryGetInt32(out int userId))
            {
                // In a bulk operation, we might want to cache user existence checks to avoid hitting the DB for every single log.
                // For simplicity here, we still check one by one.
                var userExists = await _unitOfWork.Users.ExistsAsync(u => u.Id == userId, cancellationToken);
                if (userExists)
                {
                    log.UserId = userId;
                }
                else
                {
                    _logger.LogWarning($"Belirtilen userId ({userId}) veritabanında bulunamadı. UserId null olarak ayarlanıyor.");
                }
            }

            log.Username = (logData.TryGetProperty("username", out var usernameElement) && usernameElement.ValueKind != JsonValueKind.Null)
                ? usernameElement.GetString() ?? "admin"
                : "admin";

            log.ActivityType = (logData.TryGetProperty("activityType", out var activityTypeElement) && activityTypeElement.ValueKind != JsonValueKind.Null)
                ? activityTypeElement.GetString() ?? "unknown"
                : "unknown";

            log.Description = (logData.TryGetProperty("description", out var descriptionElement) && descriptionElement.ValueKind != JsonValueKind.Null)
                ? descriptionElement.GetString() ?? "No description"
                : "No description";

            log.Timestamp = (logData.TryGetProperty("timestamp", out var timestampElement) && timestampElement.ValueKind != JsonValueKind.Null && timestampElement.TryGetDateTime(out var timestamp))
                ? timestamp
                : DateTime.UtcNow;

            var additionalInfo = new Dictionary<string, object>();
            if (logData.TryGetProperty("status", out var statusElement) && statusElement.ValueKind != JsonValueKind.Null)
            {
                additionalInfo["status"] = statusElement.GetString() ?? "info";
            }
            if (logData.TryGetProperty("details", out var detailsElement) && detailsElement.ValueKind != JsonValueKind.Null)
            {
                additionalInfo["details"] = detailsElement.ToString();
            }
            if (logData.TryGetProperty("ipAddress", out var ipAddressElement) && ipAddressElement.ValueKind != JsonValueKind.Null)
            {
                additionalInfo["ipAddress"] = ipAddressElement.GetString() ?? string.Empty;
            }
            log.AdditionalInfo = JsonSerializer.Serialize(additionalInfo);

            return log;
        }
    }
} 