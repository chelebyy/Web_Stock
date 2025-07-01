using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.ActivityLogs.Commands.CreateActivityLog;
using Stock.Application.Features.ActivityLogs.Commands.CreateBulkActivityLogs;
using Stock.Application.Features.ActivityLogs.Queries.GetActivityLogs;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Stock.Application.Constants;


namespace Stock.API.Controllers
{
    /// <summary>
    /// Manages activity logs within the system.
    /// </summary>
    [ApiController]
    [Route("api/logs")]
    [Authorize(Roles = RoleNames.Admin)]
    public class ActivityLogController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ActivityLogController> _logger;

        public ActivityLogController(IMediator mediator, ILogger<ActivityLogController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of activity logs. (Admin only)
        /// </summary>
        /// <param name="query">The query parameters for filtering and pagination.</param>
        /// <returns>A paginated list of activity logs.</returns>
        /// <response code="200">Returns the paginated list of activity logs.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("activity")]
        [ProducesResponseType(typeof(PagedResponse<ActivityLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActivityLogs([FromQuery] GetActivityLogsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Aktivite logları alınırken bir hata oluştu.");
                return StatusCode(500, "Aktivite logları alınırken bir hata oluştu.");
            }
        }

        /// <summary>
        /// Creates a single activity log. (Admin only)
        /// </summary>
        /// <remarks>
        /// The 'logData' should be a JSON object representing the activity. 
        /// The structure is flexible, but it should contain relevant information about the logged activity.
        /// Example: 
        /// { 
        ///   "UserId": "123", 
        ///   "Action": "UserLoggedIn", 
        ///   "Timestamp": "2024-05-15T10:30:00Z" 
        /// }
        /// </remarks>
        /// <param name="logData">A JSON element containing the log data.</param>
        /// <returns>A confirmation message and the created log.</returns>
        /// <response code="200">Returns a confirmation message and the created log.</response>
        /// <response code="400">If the log data is invalid.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost("activity")]
        [ProducesResponseType(typeof(ActivityLogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateActivityLog([FromBody] JsonElement logData)
        {
            try
            {
                var command = new CreateActivityLogCommand { LogData = logData };
                var result = await _mediator.Send(command);
                return Ok(new { message = "Log başarıyla kaydedildi", log = result });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Aktivite logu kaydedilirken hata oluştu.");
                return StatusCode(500, new { error = "Aktivite logu kaydedilirken bir hata oluştu.", details = ex.Message });
            }
        }

        /// <summary>
        /// Creates multiple activity logs in a single request. (Admin only)
        /// </summary>
        /// <remarks>
        /// The 'logsData' should be an array of JSON objects, each representing an activity log.
        /// </remarks>
        /// <param name="logsData">A list of JSON elements containing the log data for multiple entries.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">Returns a confirmation message.</response>
        /// <response code="400">If the request payload is invalid.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost("bulk-activity")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBulkActivityLogs([FromBody] List<JsonElement> logsData)
        {
            try
            {
                var command = new CreateBulkActivityLogsCommand { LogsData = logsData };
                await _mediator.Send(command);
                return Ok(new { message = "Toplu loglar başarıyla kaydedildi." });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Toplu aktivite logları kaydedilirken hata oluştu.");
                return StatusCode(500, new { error = "Toplu aktivite logları kaydedilirken bir hata oluştu.", details = ex.Message });
            }
        }
    }
} 