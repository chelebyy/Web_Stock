using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Interfaces;
using Stock.Application.Common.Models;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerManager _logger;

        public ReportsController(
            IReportService reportService,
            ICurrentUserService currentUserService,
            ILoggerManager logger)
        {
            _reportService = reportService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        /// <summary>
        /// Yeni bir rapor oluşturma isteği ekler
        /// </summary>
        /// <param name="request">Rapor isteği</param>
        /// <returns>Oluşturulan rapor isteğinin ID'si</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> QueueReport([FromBody] QueueReportRequest request)
        {
            try
            {
                _logger.LogInfo($"Yeni rapor isteği: {request.ReportType}");
                
                var userId = _currentUserService.UserId?.ToString();
                var reportId = await _reportService.QueueReportAsync(request.ReportType, request.Parameters, userId);
                
                return Ok(new { ReportId = reportId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Rapor oluşturma isteği sırasında hata: {ex.Message}");
                return BadRequest(new { Error = "Rapor isteği oluşturulamadı.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Rapor durumunu getirir
        /// </summary>
        /// <param name="reportId">Rapor ID</param>
        /// <returns>Rapor durumu</returns>
        [HttpGet("{reportId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReportStatus(string reportId)
        {
            try
            {
                var status = await _reportService.GetReportStatusAsync(reportId);
                return Ok(new { Status = status.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Rapor durumu getirme sırasında hata: {ex.Message}");
                return NotFound(new { Error = "Rapor bulunamadı.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının raporlarını getirir
        /// </summary>
        /// <returns>Kullanıcının raporları</returns>
        [HttpGet("my-reports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyReports()
        {
            try
            {
                var userId = _currentUserService.UserId?.ToString();
                var reports = await _reportService.GetUserReportsAsync(userId);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kullanıcı raporları getirme sırasında hata: {ex.Message}");
                return BadRequest(new { Error = "Raporlar getirilemedi.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Rapor dosyasını indirir
        /// </summary>
        /// <param name="reportId">Rapor ID</param>
        /// <returns>Rapor dosyası</returns>
        [HttpGet("{reportId}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DownloadReport(string reportId)
        {
            try
            {
                var reportFile = await _reportService.DownloadReportAsync(reportId);
                
                if (reportFile == null)
                {
                    return NotFound(new { Error = "Rapor dosyası bulunamadı." });
                }
                
                return File(reportFile.Content, reportFile.ContentType, reportFile.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Rapor indirme sırasında hata: {ex.Message}");
                return NotFound(new { Error = "Rapor dosyası bulunamadı.", Details = ex.Message });
            }
        }
    }

    /// <summary>
    /// Rapor oluşturma isteği
    /// </summary>
    public class QueueReportRequest
    {
        /// <summary>
        /// Rapor tipi
        /// </summary>
        public string ReportType { get; set; }
        
        /// <summary>
        /// Rapor parametreleri
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
} 