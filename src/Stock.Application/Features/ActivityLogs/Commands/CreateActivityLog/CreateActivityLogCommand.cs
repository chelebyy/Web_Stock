using MediatR;
using Stock.Application.Models.DTOs;
using System;
using System.Text.Json;

namespace Stock.Application.Features.ActivityLogs.Commands.CreateActivityLog
{
    public class CreateActivityLogCommand : IRequest<ActivityLogDto>
    {
        // Using object to maintain flexibility from the original controller,
        // but in a real-world scenario, a strongly-typed DTO would be better.
        public JsonElement LogData { get; set; }
    }
} 