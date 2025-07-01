using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.ActivityLogs.Commands.CreateActivityLog
{
    public class CreateActivityLogCommandHandler : IRequestHandler<CreateActivityLogCommand, ActivityLogDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateActivityLogCommandHandler> _logger;

        public CreateActivityLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateActivityLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActivityLogDto> Handle(CreateActivityLogCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tekil aktivite log isteği işleniyor.");

            var log = await ParseLogData(request.LogData, cancellationToken);

            var addedLog = await _unitOfWork.ActivityLogs.AddAsync(log, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Veritabanına log kaydedildi. Log ID: {addedLog.Id}, UserId: {addedLog.UserId}, ActivityType: {addedLog.ActivityType}");

            // We need to load the user to get the FullName for the DTO
            if (addedLog.UserId.HasValue)
            {
                addedLog.User = await _unitOfWork.Users.GetByIdAsync(addedLog.UserId.Value, cancellationToken);
            }

            return _mapper.Map<ActivityLogDto>(addedLog);
        }
        
        private async Task<ActivityLog> ParseLogData(JsonElement logData, CancellationToken cancellationToken)
        {
            var log = new ActivityLog();

            if (logData.TryGetProperty("userId", out var userIdElement) && userIdElement.ValueKind != JsonValueKind.Null && userIdElement.TryGetInt32(out int userId))
            {
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