using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Features.Users.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Stock.Application.Features.Users.Commands.CreateUserCommand command)
        {
            try
            {
                _logger.LogInformation("Kullanıcı ekleme isteği başlatıldı: {Username}", command.Username);
                var result = await _mediator.Send(command);
                _logger.LogInformation("Kullanıcı başarıyla oluşturuldu: {Username}, ID: {Id}", result.Username, result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning("Kullanıcı ekleme işlemi sırasında doğrulama hatası: {Message}", errorMessage);
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains("sicil numarası zaten kullanılmaktadır"))
                {
                    return BadRequest(new { error = errorMessage });
                }
                
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı ekleme işlemi sırasında hata oluştu");
                return StatusCode(500, new { error = "Kullanıcı eklenirken bir hata oluştu." });
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
                    _logger.LogWarning("ID uyuşmazlığı: URL'deki {UrlId} ile gönderilen {CommandId} eşleşmiyor", id, command.Id);
                    return BadRequest(new { error = "URL'deki ID ile request body'deki ID eşleşmiyor." });
                }
                
                _logger.LogInformation("Kullanıcı güncelleme isteği başlatıldı: ID: {Id}, Username: {Username}", id, command.Username);
                var result = await _mediator.Send(command);
                
                if (result == null)
                {
                    _logger.LogWarning("Güncellenecek kullanıcı bulunamadı: ID: {Id}", id);
                    return NotFound(new { error = $"ID: {id} olan kullanıcı bulunamadı." });
                }
                
                _logger.LogInformation("Kullanıcı başarıyla güncellendi: ID: {Id}, Username: {Username}", result.Id, result.Username);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning("Kullanıcı güncelleme işlemi sırasında doğrulama hatası: {Message}", errorMessage);
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains("sicil numarası zaten"))
                {
                    return BadRequest(new { error = errorMessage });
                }
                
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme işlemi sırasında hata oluştu");
                return StatusCode(500, new { error = "Kullanıcı güncellenirken bir hata oluştu." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Kullanıcı silme isteği başlatıldı: ID: {Id}", id);
                var command = new Stock.Application.Features.Users.Commands.DeleteUserCommand { Id = id };
                var result = await _mediator.Send(command);
                
                if (!result)
                {
                    _logger.LogWarning("Silinecek kullanıcı bulunamadı: ID: {Id}", id);
                    return NotFound(new { error = $"ID: {id} olan kullanıcı bulunamadı." });
                }
                
                _logger.LogInformation("Kullanıcı başarıyla silindi: ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silme işlemi sırasında hata oluştu");
                return StatusCode(500, new { error = "Kullanıcı silinirken bir hata oluştu." });
            }
        }
    }
} 