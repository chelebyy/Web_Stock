using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Admin.Commands.CleanupUnusedPermissions;
using System;
using System.Threading.Tasks;


namespace Stock.API.Controllers
{
    /// <summary>
    /// Provides administrative functionalities. Requires Admin role.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for sending commands and queries.</param>
        /// <param name="logger">The logger for logging information and errors.</param>
        public AdminController(IMediator mediator, ILogger<AdminController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Deletes all unused page permissions from the system.
        /// </summary>
        /// <remarks>
        /// This endpoint triggers a process to find and remove permissions that are not associated with any roles or users.
        /// It's a cleanup operation to maintain data integrity.
        /// </remarks>
        /// <returns>A success message if the cleanup is successful.</returns>
        /// <response code="200">Unused permissions were successfully cleaned up.</response>
        /// <response code="401">Unauthorized access. User is not authenticated.</response>
        /// <response code="403">Forbidden access. User is not in the 'Admin' role.</response>
        /// <response code="500">An internal server error occurred during the cleanup process.</response>
        [HttpDelete("cleanup-unused-permissions")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CleanupUnusedPermissions()
        {
            try
            {
                _logger.LogInformation("Kullanılmayan sayfa izinlerini temizleme isteği alındı.");
                await _mediator.Send(new CleanupUnusedPermissionsCommand());
                _logger.LogInformation("Kullanılmayan sayfa izinleri başarıyla temizlendi.");
                return Ok(new { message = "Kullanılmayan sayfa izinleri başarıyla temizlendi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzinler temizlenirken bir hata oluştu");
                return StatusCode(500, "İzinler temizlenirken bir hata oluştu: " + ex.Message);
            }
        }
    }
} 