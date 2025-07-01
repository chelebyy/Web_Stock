using MediatR;
using System.Collections.Generic;
using System.Text.Json;

namespace Stock.Application.Features.ActivityLogs.Commands.CreateBulkActivityLogs
{
    public class CreateBulkActivityLogsCommand : IRequest
    {
        public List<JsonElement> LogsData { get; set; }
    }
} 