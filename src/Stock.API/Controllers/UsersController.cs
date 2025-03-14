using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Features.Users.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using Stock.Infrastructure.Logging;
using Stock.Application.Features.Users.Queries.GetPaginatedUsers;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        // Sabit hata mesajları
        private const string ERROR_USER_NOT_FOUND = "ID: {0} olan kullanıcı bulunamadı.";
        private const string ERROR_USER_CREATE = "Kullanıcı eklenirken bir hata oluştu.";
        private const string ERROR_USER_UPDATE = "Kullanıcı güncellenirken bir hata oluştu.";
        private const string ERROR_USER_DELETE = "Kullanıcı silinirken bir hata oluştu.";
        private const string ERROR_ID_MISMATCH = "URL'deki ID ile request body'deki ID eşleşmiyor.";
        private const string ERROR_VALIDATION = "{0}";

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
            _logger.LogInfo($"{result.Count} kullanıcı başarıyla getirildi.");
            return Ok(result);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInfo($"Sayfalanmış kullanıcılar getiriliyor. Sayfa: {pageNumber}, Sayfa Boyutu: {pageSize}");
            
            var query = new GetPaginatedUsersQuery
            {
                PageNumber = pageNumber < 1 ? 1 : pageNumber,
                PageSize = pageSize < 1 ? 10 : (pageSize > 50 ? 50 : pageSize)
            };
            
            var result = await _mediator.Send(query);
            
            _logger.LogInfo($"Toplam {result.TotalCount} kullanıcıdan {result.Items.Count()} kullanıcı başarıyla getirildi. Sayfa: {pageNumber}/{result.TotalPages}");
            
            return Ok(result);
        }

        [HttpGet("summaries")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSummaries()
        {
            _logger.LogInfo("Kullanıcı özetleri getiriliyor.");
            
            var query = new GetUserSummariesQuery();
            var result = await _mediator.Send(query);
            
            _logger.LogInfo($"{result.Count()} kullanıcı özeti başarıyla getirildi.");
            
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
                _logger.LogWarn(string.Format(ERROR_USER_NOT_FOUND, id));
                return NotFound(string.Format(ERROR_USER_NOT_FOUND, id));
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
                _logger.LogError(ERROR_USER_CREATE, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ERROR_USER_CREATE);
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
                    _logger.LogWarn(ERROR_ID_MISMATCH);
                    return BadRequest(ERROR_ID_MISMATCH);
                }

                _logger.LogInfo($"ID: {id} olan kullanıcı güncelleniyor.");
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarn(string.Format(ERROR_USER_NOT_FOUND, id));
                    return NotFound(string.Format(ERROR_USER_NOT_FOUND, id));
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
                _logger.LogError(ERROR_USER_UPDATE, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ERROR_USER_UPDATE);
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
                    _logger.LogWarn(string.Format(ERROR_USER_NOT_FOUND, id));
                    return NotFound(string.Format(ERROR_USER_NOT_FOUND, id));
                }

                _logger.LogInfo($"ID: {id} olan kullanıcı başarıyla silindi.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ERROR_USER_DELETE, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ERROR_USER_DELETE);
            }
        }
    }
} 