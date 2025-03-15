using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Models.DTOs;
using Stock.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Stock.Application.Features.Users.Queries.GetPaginatedUsers;
using System.Diagnostics;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        // Hata mesajları için sabitler
        private const string USER_NOT_FOUND = "Kullanıcı bulunamadı.";
        private const string USER_CREATION_ERROR = "Kullanıcı oluşturulurken bir hata oluştu.";
        private const string USER_UPDATE_ERROR = "Kullanıcı güncellenirken bir hata oluştu.";
        private const string USER_DELETE_ERROR = "Kullanıcı silinirken bir hata oluştu.";
        private const string ID_MISMATCH_ERROR = "URL'deki ID ile gönderilen ID eşleşmiyor.";

        public UsersController(IMediator mediator, ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInfo("Tüm kullanıcılar getiriliyor.");
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            _logger.LogInfo($"{result.Count()} kullanıcı başarıyla getirildi.");
            return Ok(result);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string usernameFilter = null,
            [FromQuery] string sicilFilter = null,
            [FromQuery] int? roleIdFilter = null,
            [FromQuery] bool? isActiveFilter = null,
            [FromQuery] bool? isAdminFilter = null,
            [FromQuery] string sortBy = "Username",
            [FromQuery] bool sortAscending = true)
        {
            _logger.LogInfo($"Sayfalanmış kullanıcılar getiriliyor. Sayfa: {pageNumber}, Sayfa Boyutu: {pageSize}, Filtreler: " +
                $"Username={usernameFilter}, Sicil={sicilFilter}, RoleId={roleIdFilter}, IsActive={isActiveFilter}, IsAdmin={isAdminFilter}, " +
                $"Sıralama: {sortBy} {(sortAscending ? "Artan" : "Azalan")}");
            
            // Performans ölçümü için Stopwatch başlat
            var stopwatch = Stopwatch.StartNew();
            
            var query = new GetPaginatedUsersQuery
            {
                PageNumber = pageNumber < 1 ? 1 : pageNumber,
                PageSize = pageSize < 1 ? 10 : (pageSize > 50 ? 50 : pageSize),
                UsernameFilter = usernameFilter,
                SicilFilter = sicilFilter,
                RoleIdFilter = roleIdFilter,
                IsActiveFilter = isActiveFilter,
                IsAdminFilter = isAdminFilter,
                SortBy = sortBy,
                SortAscending = sortAscending
            };
            
            var result = await _mediator.Send(query);
            
            // Stopwatch'ı durdur ve geçen süreyi hesapla
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            _logger.LogInfo($"Toplam {result.TotalCount} kullanıcıdan {result.Items.Count()} kullanıcı {elapsedMs}ms sürede başarıyla getirildi. Sayfa: {pageNumber}/{result.TotalPages}");
            
            // Response header'a performans bilgisini ekle
            Response.Headers.Add("X-Response-Time-Ms", elapsedMs.ToString());
            
            return Ok(result);
        }

        [HttpGet("summaries")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSummaries()
        {
            _logger.LogInfo("Kullanıcı özetleri getiriliyor.");
            
            // Performans ölçümü için Stopwatch başlat
            var stopwatch = Stopwatch.StartNew();
            
            var query = new GetUserSummariesQuery();
            var result = await _mediator.Send(query);
            
            // Stopwatch'ı durdur ve geçen süreyi hesapla
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            _logger.LogInfo($"{result.Count()} kullanıcı özeti {elapsedMs}ms sürede başarıyla getirildi.");
            
            // Response header'a performans bilgisini ekle
            Response.Headers.Add("X-Response-Time-Ms", elapsedMs.ToString());
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInfo($"ID: {id} olan kullanıcı getiriliyor.");
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                _logger.LogWarn(string.Format(USER_NOT_FOUND, id));
                return NotFound(string.Format(USER_NOT_FOUND, id));
            }

            _logger.LogInfo($"ID: {id} olan kullanıcı başarıyla getirildi.");
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Stock.Application.Features.Users.Commands.CreateUserCommand command)
        {
            try
            {
                _logger.LogInfo("Yeni kullanıcı oluşturuluyor.");
                var result = await _mediator.Send(command);
                _logger.LogInfo($"ID: {result.Id} olan kullanıcı başarıyla oluşturuldu.");
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex) when (ex.Message.Contains("unique constraint"))
            {
                _logger.LogError($"Kullanıcı oluşturulurken benzersizlik hatası: {ex.Message}");
                return Conflict("Bu e-posta adresi veya kullanıcı adı zaten kullanılıyor.");
            }
            catch (Exception ex)
            {
                _logger.LogError(USER_CREATION_ERROR, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, USER_CREATION_ERROR);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Stock.Application.Features.Users.Commands.UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    _logger.LogWarn(ID_MISMATCH_ERROR);
                    return BadRequest(ID_MISMATCH_ERROR);
                }

                _logger.LogInfo($"ID: {id} olan kullanıcı güncelleniyor.");
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarn(string.Format(USER_NOT_FOUND, id));
                    return NotFound(string.Format(USER_NOT_FOUND, id));
                }

                _logger.LogInfo($"ID: {id} olan kullanıcı başarıyla güncellendi.");
                return Ok(result);
            }
            catch (Exception ex) when (ex.Message.Contains("unique constraint"))
            {
                _logger.LogError($"Kullanıcı güncellenirken benzersizlik hatası: {ex.Message}");
                return Conflict("Bu e-posta adresi veya kullanıcı adı zaten kullanılıyor.");
            }
            catch (Exception ex)
            {
                _logger.LogError(USER_UPDATE_ERROR, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, USER_UPDATE_ERROR);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"ID: {id} olan kullanıcı siliniyor.");
                var command = new DeleteUserCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result)
                {
                    _logger.LogWarn(string.Format(USER_NOT_FOUND, id));
                    return NotFound(string.Format(USER_NOT_FOUND, id));
                }

                _logger.LogInfo($"ID: {id} olan kullanıcı başarıyla silindi.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(USER_DELETE_ERROR, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, USER_DELETE_ERROR);
            }
        }
    }
} 